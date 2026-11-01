namespace PracticaU3Act3;

// The form only handles events and shows results; the logic lives in BrowserHistory.
// The pages are simulated: nothing is downloaded and the site is not checked.
public partial class Form1 : Form
{
    private readonly BrowserHistory _history = new();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        ShowMessage("Navegador listo: escribe una dirección o elige un enlace de ejemplo.");
        RefreshView();
        ActiveControl = txtAddress;
    }

    // ---------- Browser ----------

    private void btnGo_Click(object? sender, EventArgs e) => Navigate(txtAddress.Text);

    // The five example buttons share this handler: the address to visit is the text of the button.
    private void btnSample_Click(object? sender, EventArgs e)
    {
        if (sender is Button button)
            Navigate(button.Text);
    }

    private void txtAddress_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
            return;
        e.SuppressKeyPress = true;
        Navigate(txtAddress.Text);
    }

    private void btnBack_Click(object? sender, EventArgs e) =>
        Run(() => $"Atrás: ahora estás en {_history.Back()}. La página que dejaste pasó a la pila de adelante.");

    private void btnForward_Click(object? sender, EventArgs e) =>
        Run(() => $"Adelante: ahora estás en {_history.Forward()}. La página que dejaste volvió a la pila de atrás.");

    private void btnClear_Click(object? sender, EventArgs e) =>
        Run(() =>
        {
            _history.Clear();
            return "Historial vaciado: no hay página actual y las dos pilas quedaron vacías.";
        });

    private void Navigate(string address)
    {
        int discarded = _history.ForwardCount;
        Run(() =>
        {
            if (!_history.Visit(address))
                return $"{_history.Current} ya es la página actual: no se agrega otra vez al historial.";

            string message = $"Visit: {_history.Current} es ahora la página actual.";
            if (_history.BackCount > 0)
                message += " La anterior pasó a la pila de atrás.";
            if (discarded > 0)
                message += $" Se vació la pila de adelante ({Pages(discarded)}).";
            return message;
        });
        txtAddress.Focus();
        txtAddress.SelectAll();
    }

    // Runs an operation; the expected errors (invalid address, nothing to go back or forward to) are shown as a message.
    private void Run(Func<string> operation)
    {
        try
        {
            ShowMessage(operation());
            txtAddress.Text = _history.Current ?? string.Empty;
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            ShowMessage(ex.Message, error: true);
        }
        RefreshView();
    }

    private void RefreshView()
    {
        btnBack.Enabled = _history.CanGoBack;
        btnForward.Enabled = _history.CanGoForward;

        string? current = _history.Current;
        lblPageTitle.Text = current == null ? "Página en blanco" : $"Página simulada de {HostOf(current)}";
        lblPageUrl.Text = current ?? "Todavía no has visitado ninguna página.";
        lblInfo.Text = $"Atrás: {_history.BackCount}   |   Adelante: {_history.ForwardCount}   |   Actual: {current ?? "ninguna"}";
        picStacks.Invalidate();
    }

    // The drawing receives copies of the stacks, so the form never touches the nodes.
    private void picStacks_Paint(object? sender, PaintEventArgs e) =>
        StackDrawing.Draw(e.Graphics, picStacks.ClientSize, _history.Current, _history.BackItems(), _history.ForwardItems());

    private void picStacks_Resize(object? sender, EventArgs e) => picStacks.Invalidate();

    // ---------- Tests and measurements ----------

    // Tests and measurements take a while, so they run on another thread to keep the window responsive.
    private async void btnTests_Click(object? sender, EventArgs e)
    {
        btnTests.Enabled = false;
        txtTests.Text = "Ejecutando pruebas y mediciones...";
        try
        {
            txtTests.Text = await Task.Run(BuildTestReport);
        }
        catch (Exception ex)
        {
            txtTests.Text = "Error inesperado durante las pruebas: " + ex.Message;
        }
        finally
        {
            btnTests.Enabled = true;
        }
    }

    private static string BuildTestReport()
    {
        List<TestResult> results = TestBench.RunCases();
        var lines = new List<string>();
        foreach (TestResult result in results)
            lines.Add($"[{(result.Passed ? "OK" : "FALLA")}] {result.Case}: {result.Detail}");

        lines.Add(string.Empty);
        lines.Add($"{results.Count(r => r.Passed)} de {results.Count} casos aprobados.");
        lines.Add(string.Empty);
        lines.Add(TestBench.RunMeasurements());
        return string.Join(Environment.NewLine, lines);
    }

    // ---------- Helpers ----------

    private void ShowMessage(string message, bool error = false)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = error ? Color.FromArgb(180, 30, 30) : Color.Black;
    }

    private static string Pages(int count) => count == 1 ? "1 página" : $"{count} páginas";

    // Site name of an address: what comes after "://" and before the first "/", "?" or "#".
    private static string HostOf(string address)
    {
        int schemeEnd = address.IndexOf("://", StringComparison.Ordinal);
        string rest = schemeEnd >= 0 ? address[(schemeEnd + 3)..] : address;
        int end = rest.IndexOfAny(new[] { '/', '?', '#' });
        return end >= 0 ? rest[..end] : rest;
    }
}