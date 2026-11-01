using System.Drawing.Drawing2D;

namespace PracticaU3Act3;

// Draws the browser history: the "back" stack on the left, the current page in the middle and the
// "forward" stack on the right. Each stack is drawn vertically, standing on its base line, with the
// top element marked by a "tope" label and an arrow. If a stack does not fit, the middle boxes are
// replaced by "..." (the top ones and the bottom one are always visible).
public static class StackDrawing
{
    private const int MinBoxWidth = 90;
    private const int MaxBoxWidth = 220;
    private const int BoxHeight = 30;
    private const int BoxGap = 6;
    private const int ColumnGap = 40;
    private const int Margin = 16;
    private const int TitleHeight = 28;
    private const int LabelOffset = 20;   // distance between a label and the box it points to
    private const int LabelZone = 38;     // room above the top box for the label and its arrow
    private const int BottomZone = 34;    // room below the base line for the page counter
    private const int BaseGap = 4;        // space between the base line and the lowest box

    // back and forward are ordered from the top of the stack to the bottom.
    public static void Draw(Graphics g, Size area, string? current, string[] back, string[] forward)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        using var tools = new Tools();

        int boxWidth = Math.Clamp((area.Width - 2 * Margin - 2 * ColumnGap) / 3, MinBoxWidth, MaxBoxWidth);
        float groupWidth = 3 * boxWidth + 2 * ColumnGap;
        float left = Math.Max(Margin, (area.Width - groupWidth) / 2f);
        float backX = left;
        float currentX = backX + boxWidth + ColumnGap;
        float forwardX = currentX + boxWidth + ColumnGap;

        float baseY = area.Height - BottomZone;
        int maxSlots = Math.Max(3, (int)((baseY - BaseGap - Margin - TitleHeight - LabelZone) / (BoxHeight + BoxGap)));

        DrawTitle(g, "Pila Atrás", backX, boxWidth, tools);
        DrawTitle(g, "Página actual", currentX, boxWidth, tools);
        DrawTitle(g, "Pila Adelante", forwardX, boxWidth, tools);

        DrawStack(g, back, backX, boxWidth, baseY, maxSlots, tools);
        DrawStack(g, forward, forwardX, boxWidth, baseY, maxSlots, tools);
        DrawCurrent(g, current, currentX, boxWidth, baseY, tools);
    }

    private static void DrawTitle(Graphics g, string title, float x, int boxWidth, Tools t) =>
        g.DrawString(title, t.Bold, t.Black, new RectangleF(x, Margin / 2f, boxWidth, TitleHeight), t.Center);

    private static void DrawStack(Graphics g, string[] values, float x, int boxWidth, float baseY, int maxSlots, Tools t)
    {
        g.DrawLine(t.BasePen, x - 8, baseY, x + boxWidth + 8, baseY);

        string counter = values.Length == 1 ? "1 página" : $"{values.Length} páginas";
        g.DrawString(counter, t.Small, t.Black, new RectangleF(x, baseY + 6, boxWidth, 20), t.Center);

        if (values.Length == 0)
        {
            var emptyRect = new RectangleF(x, baseY - BaseGap - BoxHeight, boxWidth, BoxHeight);
            DrawBox(g, emptyRect, "vacía", t.DashedPen, t.Gray, t);
            return;
        }

        List<int> slots = ChooseSlots(values.Length, maxSlots);
        float pitch = BoxHeight + BoxGap;
        float top = baseY - BaseGap - (slots.Count * pitch - BoxGap);
        RectangleF topRect = RectangleF.Empty;

        for (int s = 0; s < slots.Count; s++)
        {
            var rect = new RectangleF(x, top + s * pitch, boxWidth, BoxHeight);
            if (slots[s] >= 0)
                DrawBox(g, rect, values[slots[s]], t.OutlinePen, t.Black, t);
            else
                g.DrawString("...", t.Bold, t.Black, rect, t.Center);

            if (s == 0)
                topRect = rect;
        }

        DrawLabelAbove(g, topRect, "tope", t);
    }

    private static void DrawCurrent(Graphics g, string? current, float x, int boxWidth, float baseY, Tools t)
    {
        var rect = new RectangleF(x, baseY - BaseGap - BoxHeight, boxWidth, BoxHeight);
        if (current == null)
            DrawBox(g, rect, "(sin página)", t.DashedPen, t.Gray, t);
        else
            DrawBox(g, rect, current, t.ThickPen, t.Black, t);

        DrawLabelAbove(g, rect, "actual", t);
    }

    // Returns the indexes of the elements to draw, from the top; -1 stands for the "..." slot.
    private static List<int> ChooseSlots(int count, int maxSlots)
    {
        var slots = new List<int>();
        if (count <= maxSlots)
        {
            for (int i = 0; i < count; i++)
                slots.Add(i);
            return slots;
        }

        int shown = maxSlots - 2;
        for (int i = 0; i < shown; i++)
            slots.Add(i);
        slots.Add(-1);
        slots.Add(count - 1);
        return slots;
    }

    private static void DrawBox(Graphics g, RectangleF rect, string text, Pen outline, Brush textBrush, Tools t)
    {
        using var path = RoundedRectangle(rect, 8f);
        g.DrawPath(outline, path);
        g.DrawString(text, t.Text, textBrush, new RectangleF(rect.X + 4, rect.Y, rect.Width - 8, rect.Height), t.Center);
    }

    private static void DrawLabelAbove(Graphics g, RectangleF box, string label, Tools t)
    {
        float centerX = box.X + box.Width / 2f;
        SizeF size = g.MeasureString(label, t.Bold);
        float textY = box.Y - LabelOffset - size.Height;
        g.DrawString(label, t.Bold, t.Black, centerX - size.Width / 2, textY);
        g.DrawLine(t.ArrowPen, centerX, textY + size.Height, centerX, box.Y - 2);
    }

    private static GraphicsPath RoundedRectangle(RectangleF rect, float radius)
    {
        float d = radius * 2;
        var path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    // Fonts, pens and brushes of one drawing; they are created and released together on every paint.
    private sealed class Tools : IDisposable
    {
        private readonly AdjustableArrowCap _cap = new(4, 5);

        public Font Text { get; } = new("Arial", 9.5f);
        public Font Bold { get; } = new("Arial", 10f, FontStyle.Bold);
        public Font Small { get; } = new("Arial", 9f);

        public Pen OutlinePen { get; } = new(Color.Black, 1.5f);
        public Pen ThickPen { get; } = new(Color.Black, 3f);
        public Pen BasePen { get; } = new(Color.Black, 2.5f);
        public Pen DashedPen { get; } = new(Color.Gray, 1.5f) { DashStyle = DashStyle.Dash };
        public Pen ArrowPen { get; }

        public Brush Black { get; } = new SolidBrush(Color.Black);
        public Brush Gray { get; } = new SolidBrush(Color.Gray);

        public StringFormat Center { get; } = new()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter,
            FormatFlags = StringFormatFlags.NoWrap
        };

        public Tools()
        {
            ArrowPen = new Pen(Color.Black, 1.5f) { CustomEndCap = _cap };
        }

        public void Dispose()
        {
            Text.Dispose();
            Bold.Dispose();
            Small.Dispose();
            OutlinePen.Dispose();
            ThickPen.Dispose();
            BasePen.Dispose();
            DashedPen.Dispose();
            ArrowPen.Dispose();
            _cap.Dispose();
            Black.Dispose();
            Gray.Dispose();
            Center.Dispose();
        }
    }
}
