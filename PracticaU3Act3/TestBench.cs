using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PracticaU3Act3;

public sealed record TestResult(string Case, bool Passed, string Detail);

// Test cases and measurements for the browser history.
// The expected values are written inside each case. List<string> appears only as a reference model
// (the random-operations test and the measurements); BrowserHistory never uses it.
public static class TestBench
{
    private const string One = "https://uno.com";
    private const string Two = "https://dos.com";
    private const string Three = "https://tres.com";
    private const string Four = "https://cuatro.com";

    // ================= Test cases =================

    public static List<TestResult> RunCases()
    {
        Func<TestResult>[] cases =
        {
            History_InitialState,
            History_FirstVisit,
            History_ThreeVisits,
            History_BackMovesCurrentToForward,
            History_ForwardRestoresState,
            History_BackWithoutHistory,
            History_ForwardWithoutHistory,
            History_VisitAfterBackClearsForward,
            History_VisitSamePageDoesNotDuplicate,
            History_InvalidAddresses,
            History_Normalization,
            History_Clear,
            History_RandomOperations,
            History_Large
        };

        var results = new List<TestResult>();
        foreach (var testCase in cases)
        {
            try
            {
                results.Add(testCase());
            }
            catch (Exception ex)
            {
                results.Add(new TestResult(testCase.Method.Name, false, "excepción inesperada: " + ex.Message));
            }
        }
        return results;
    }

    private static TestResult History_InitialState()
    {
        var history = new BrowserHistory();
        bool ok = history.Current == null
                  && history.BackCount == 0 && history.ForwardCount == 0
                  && !history.CanGoBack && !history.CanGoForward
                  && history.BackItems().Length == 0 && history.ForwardItems().Length == 0;
        return new TestResult("Historial: estado inicial", ok,
            "sin página actual, ambas pilas vacías y ni Atrás ni Adelante disponibles");
    }

    private static TestResult History_FirstVisit()
    {
        var history = new BrowserHistory();
        bool visited = history.Visit("ejemplo.com");
        bool ok = visited && history.Current == "https://ejemplo.com"
                  && history.BackCount == 0 && history.ForwardCount == 0
                  && !history.CanGoBack && !history.CanGoForward;
        return new TestResult("Historial: primera visita", ok,
            "la página queda como actual y no se empuja nada a la pila de atrás");
    }

    private static TestResult History_ThreeVisits()
    {
        var history = CreateWith(One, Two, Three);
        bool ok = history.Current == Three
                  && history.BackItems().SequenceEqual(new[] { Two, One })
                  && history.BackCount == 2 && history.ForwardCount == 0
                  && history.CanGoBack && !history.CanGoForward;
        return new TestResult("Historial: tres visitas", ok,
            "la actual es la tercera; la pila de atrás guarda (de tope a fondo) la segunda y la primera");
    }

    private static TestResult History_BackMovesCurrentToForward()
    {
        var history = CreateWith(One, Two, Three);
        string page = history.Back();
        bool ok = page == Two && history.Current == Two
                  && history.BackItems().SequenceEqual(new[] { One })
                  && history.ForwardItems().SequenceEqual(new[] { Three })
                  && history.CanGoBack && history.CanGoForward;
        return new TestResult("Historial: Atrás mueve la actual a adelante", ok,
            "tras visitar uno, dos y tres y dar Atrás: actual = dos, atrás = uno, adelante = tres");
    }

    private static TestResult History_ForwardRestoresState()
    {
        var history = CreateWith(One, Two, Three);
        string before = Snapshot(history);

        history.Back();
        history.Forward();
        bool single = Snapshot(history) == before;

        history.Back();
        history.Back();
        history.Forward();
        history.Forward();
        bool twice = Snapshot(history) == before;

        bool ok = single && twice;
        return new TestResult("Historial: Adelante restituye el estado previo a Atrás", ok,
            "Atrás + Adelante y también Atrás, Atrás, Adelante, Adelante dejan el mismo estado de antes");
    }

    private static TestResult History_BackWithoutHistory()
    {
        var empty = new BrowserHistory();
        bool emptyRejected = Throws<InvalidOperationException>(() => empty.Back(), ErrorMessages.NoPreviousPage);

        var history = CreateWith(One, Two);
        history.Back();                              // now on the first page: nothing before it
        string before = Snapshot(history);
        bool firstRejected = Throws<InvalidOperationException>(() => history.Back(), ErrorMessages.NoPreviousPage);
        bool unchanged = Snapshot(history) == before;

        bool ok = emptyRejected && firstRejected && unchanged;
        return new TestResult("Historial: Atrás sin página anterior", ok,
            "se rechaza con mensaje tanto sin ninguna visita como estando en la primera página, sin cambiar el estado");
    }

    private static TestResult History_ForwardWithoutHistory()
    {
        var empty = new BrowserHistory();
        bool emptyRejected = Throws<InvalidOperationException>(() => empty.Forward(), ErrorMessages.NoNextPage);

        var history = CreateWith(One, Two);          // never went back: forward is empty
        string before = Snapshot(history);
        bool noBackRejected = Throws<InvalidOperationException>(() => history.Forward(), ErrorMessages.NoNextPage);

        history.Back();
        history.Forward();                           // forward is empty again
        bool exhaustedRejected = Throws<InvalidOperationException>(() => history.Forward(), ErrorMessages.NoNextPage);
        bool unchanged = Snapshot(history) == before;

        bool ok = emptyRejected && noBackRejected && exhaustedRejected && unchanged;
        return new TestResult("Historial: Adelante sin página siguiente", ok,
            "se rechaza con mensaje sin visitas, sin haber retrocedido y tras agotar los Adelante, sin cambiar el estado");
    }

    private static TestResult History_VisitAfterBackClearsForward()
    {
        var history = CreateWith(One, Two, Three);
        history.Back();
        history.Back();                              // on One, forward holds Two and Three
        bool forwardFull = history.ForwardCount == 2;

        bool visited = history.Visit(Four);
        bool ok = forwardFull && visited && history.Current == Four
                  && history.ForwardCount == 0 && !history.CanGoForward
                  && history.BackItems().SequenceEqual(new[] { One })
                  && Throws<InvalidOperationException>(() => history.Forward(), ErrorMessages.NoNextPage);
        return new TestResult("Historial: visitar tras retroceder vacía adelante", ok,
            "con dos páginas en adelante, una visita nueva las descarta y Adelante ya no está disponible");
    }

    private static TestResult History_VisitSamePageDoesNotDuplicate()
    {
        var history = CreateWith(One, Two);
        string before = Snapshot(history);
        bool sameText = !history.Visit(Two);
        bool sameWithoutPrefix = !history.Visit("dos.com");     // normalizes to https://dos.com
        bool untouched = Snapshot(history) == before;

        history.Back();                              // on One, forward holds Two
        string beforeReload = Snapshot(history);
        bool reload = !history.Visit(One);
        bool forwardKept = Snapshot(history) == beforeReload && history.ForwardCount == 1;

        bool ok = sameText && sameWithoutPrefix && untouched && reload && forwardKept;
        return new TestResult("Historial: visitar la página actual no duplica", ok,
            "devuelve false y no toca las pilas, ni siquiera la de adelante, aunque se escriba sin https://");
    }

    private static TestResult History_InvalidAddresses()
    {
        var history = CreateWith(One);
        string before = Snapshot(history);

        bool ok = Throws<ArgumentException>(() => history.Visit(""), ErrorMessages.EmptyAddress)
                  && Throws<ArgumentException>(() => history.Visit("   "), ErrorMessages.EmptyAddress)
                  && Throws<ArgumentException>(() => history.Visit("https://"), ErrorMessages.EmptyAddress)
                  && Throws<ArgumentException>(() => history.Visit("mi sitio.com"), ErrorMessages.AddressWithSpaces)
                  && Throws<ArgumentException>(() => history.Visit("mi\tsitio.com"), ErrorMessages.AddressWithSpaces)
                  && Snapshot(history) == before;
        return new TestResult("Historial: direcciones inválidas", ok,
            "vacía, solo espacios, solo \"https://\" y con espacios internos se rechazan con mensaje sin cambiar el estado");
    }

    private static TestResult History_Normalization()
    {
        bool ok = Normalizes("ejemplo.com", "https://ejemplo.com")
                  && Normalizes("  ejemplo.com  ", "https://ejemplo.com")
                  && Normalizes("http://ejemplo.com", "http://ejemplo.com")
                  && Normalizes("https://ejemplo.com", "https://ejemplo.com")
                  && Normalizes("HTTP://Ejemplo.com", "HTTP://Ejemplo.com");
        return new TestResult("Historial: normalización de direcciones", ok,
            "agrega https:// si falta, recorta los extremos y respeta http:// y https:// ya escritos");
    }

    private static TestResult History_Clear()
    {
        var history = CreateWith(One, Two, Three);
        history.Back();
        history.Clear();
        bool cleared = history.Current == null
                       && history.BackCount == 0 && history.ForwardCount == 0
                       && !history.CanGoBack && !history.CanGoForward;

        bool visited = history.Visit(Four);
        bool reused = visited && history.Current == Four && history.BackCount == 0 && history.ForwardCount == 0;

        bool ok = cleared && reused;
        return new TestResult("Historial: Clear deja todo vacío", ok,
            "sin actual y con ambas pilas vacías; después se puede volver a navegar con normalidad");
    }

    private static TestResult History_RandomOperations()
    {
        const string name = "Historial: 20 000 operaciones al azar contra un modelo List<string> + índice";
        var random = new Random(12345);
        var history = new BrowserHistory();
        var model = new ReferenceModel();

        string[] pool =
        {
            "ejemplo.com", "https://uno.mx", "http://dos.mx", "tres.org/inicio", "cuatro.net",
            "https://cinco.io/a", "seis.com", "siete.com", "ocho.com", "nueve.com"
        };
        string[] invalid = { "", "   ", "a b", "https://", "x\ty" };

        int visits = 0, backs = 0, forwards = 0, rejected = 0, clears = 0;

        for (int step = 0; step < 20_000; step++)
        {
            int roll = random.Next(100);
            bool good;

            if (roll < 40)
            {
                string url = pool[random.Next(pool.Length)];
                good = history.Visit(url) == model.Visit(url);
                visits++;
            }
            else if (roll < 65)
            {
                good = model.CanGoBack
                    ? history.Back() == model.Back()
                    : Throws<InvalidOperationException>(() => history.Back(), ErrorMessages.NoPreviousPage);
                backs++;
            }
            else if (roll < 85)
            {
                good = model.CanGoForward
                    ? history.Forward() == model.Forward()
                    : Throws<InvalidOperationException>(() => history.Forward(), ErrorMessages.NoNextPage);
                forwards++;
            }
            else if (roll < 95)
            {
                // Reload: visiting the page that is already shown (or the first page if there is none yet).
                string url = model.Current ?? pool[0];
                good = history.Visit(url) == model.Visit(url);
                visits++;
            }
            else if (roll < 98)
            {
                string bad = invalid[random.Next(invalid.Length)];
                good = Throws<ArgumentException>(() => history.Visit(bad));
                rejected++;
            }
            else
            {
                history.Clear();
                model.Clear();
                good = true;
                clears++;
            }

            // Cheap comparison after every step; the full contents of both stacks every 100 steps.
            good = good && SameState(history, model, deep: step % 100 == 0);
            if (!good)
                return new TestResult(name, false, $"difiere en el paso {step}");
        }

        bool same = SameState(history, model, deep: true);
        return new TestResult(name, same,
            $"{visits} visitas, {backs} Atrás, {forwards} Adelante, {rejected} direcciones inválidas y {clears} Clear; "
            + "coinciden la página actual, los contadores y el contenido de ambas pilas");
    }

    private static TestResult History_Large()
    {
        const string name = "Historial: 100 000 retrocesos y 100 000 avances";
        const int n = 100_000;

        // n + 1 visits leave exactly n pages behind the current one.
        var urls = new string[n + 1];
        for (int i = 0; i <= n; i++)
            urls[i] = $"https://sitio{i}.com";

        var history = new BrowserHistory();
        foreach (string url in urls)
            history.Visit(url);
        bool ok = history.Current == urls[n] && history.BackCount == n && history.ForwardCount == 0;

        for (int i = n - 1; i >= 0 && ok; i--)
            ok = history.Back() == urls[i];
        ok = ok && history.BackCount == 0 && history.ForwardCount == n && !history.CanGoBack
             && Throws<InvalidOperationException>(() => history.Back(), ErrorMessages.NoPreviousPage);

        for (int i = 1; i <= n && ok; i++)
            ok = history.Forward() == urls[i];
        ok = ok && history.BackCount == n && history.ForwardCount == 0 && history.Current == urls[n]
             && Throws<InvalidOperationException>(() => history.Forward(), ErrorMessages.NoNextPage);

        return new TestResult(name, ok,
            "100 001 visitas, luego 100 000 Atrás hasta la primera página y 100 000 Adelante hasta la última, en el orden exacto");
    }

    // ---------- Helpers for the test cases ----------

    private static BrowserHistory CreateWith(params string[] urls)
    {
        var history = new BrowserHistory();
        foreach (string url in urls)
            history.Visit(url);
        return history;
    }

    // Current page and both stacks (top first) as one text, to compare states.
    private static string Snapshot(BrowserHistory history) =>
        $"{history.Current}|{string.Join(",", history.BackItems())}|{string.Join(",", history.ForwardItems())}";

    private static bool Normalizes(string input, string expected) =>
        UrlNormalizer.TryNormalize(input, out string normalized, out _) && normalized == expected;

    private static bool SameState(BrowserHistory history, ReferenceModel model, bool deep)
    {
        bool basic = history.Current == model.Current
                     && history.BackCount == model.BackCount && history.ForwardCount == model.ForwardCount
                     && history.CanGoBack == model.CanGoBack && history.CanGoForward == model.CanGoForward;
        if (!basic || !deep)
            return basic;
        return history.BackItems().SequenceEqual(model.BackItems())
               && history.ForwardItems().SequenceEqual(model.ForwardItems());
    }

    // Runs the action and checks that it throws TException (and, if given, that the message matches).
    private static bool Throws<TException>(Action action, string? message = null) where TException : Exception
    {
        try
        {
            action();
            return false;
        }
        catch (TException ex)
        {
            return message == null || ex.Message == message;
        }
    }

    // Reference model: all the pages in a List<string> and the position of the current one.
    // Back and Forward move the index; Visit has to cut the pages after the index.
    // It uses the same UrlNormalizer, so only the history structure is being compared.
    private sealed class ReferenceModel
    {
        private readonly List<string> _pages = new();
        private int _index = -1;

        public string? Current => _index >= 0 ? _pages[_index] : null;
        public int BackCount => Math.Max(_index, 0);
        public int ForwardCount => _pages.Count - 1 - _index;
        public bool CanGoBack => _index > 0;
        public bool CanGoForward => _index < _pages.Count - 1;

        public bool Visit(string url)
        {
            if (!UrlNormalizer.TryNormalize(url, out string address, out string error))
                throw new ArgumentException(error);

            if (_index >= 0 && _pages[_index] == address)
                return false;

            if (_index < _pages.Count - 1)
                _pages.RemoveRange(_index + 1, _pages.Count - _index - 1);

            _pages.Add(address);
            _index++;
            return true;
        }

        public string Back()
        {
            if (!CanGoBack)
                throw new InvalidOperationException(ErrorMessages.NoPreviousPage);
            _index--;
            return _pages[_index];
        }

        public string Forward()
        {
            if (!CanGoForward)
                throw new InvalidOperationException(ErrorMessages.NoNextPage);
            _index++;
            return _pages[_index];
        }

        public void Clear()
        {
            _pages.Clear();
            _index = -1;
        }

        // Same order as BrowserHistory: the nearest page first.
        public string[] BackItems()
        {
            var items = new string[BackCount];
            for (int i = 0; i < items.Length; i++)
                items[i] = _pages[_index - 1 - i];
            return items;
        }

        public string[] ForwardItems()
        {
            var items = new string[ForwardCount];
            for (int i = 0; i < items.Length; i++)
                items[i] = _pages[_index + 1 + i];
            return items;
        }
    }

    // ================= Measurements =================

    // Time per operation. Each time is the median of 5 runs after 2 warm-up runs.
    public static string RunMeasurements()
    {
        var sb = new StringBuilder();
#if DEBUG
        sb.AppendLine("AVISO: el programa está en Debug. Compila en Release y ejecuta con Ctrl+F5 para medir.");
        sb.AppendLine();
#endif
        AppendTimes(sb);
        sb.AppendLine();
        AppendVisitThatClearsForward(sb);
        return sb.ToString();
    }

    private static void AppendTimes(StringBuilder sb)
    {
        sb.AppendLine("Tiempo por operación en ns (mediana de 5 corridas tras 2 calentamientos)");
        sb.AppendLine("Pilas = BrowserHistory (dos pilas dinámicas); Modelo = List<string> + índice con la misma normalización.");
        sb.AppendLine("Las direcciones se crean antes de medir. Visit incluye revisar la dirección.");
        sb.AppendLine("En las pilas, Atrás y Adelante crean un nodo en la otra pila y sueltan el anterior.");
        sb.AppendLine();

        string header = $"{"n",12}{"Visit pilas",16}{"Visit modelo",16}{"Atrás pilas",16}{"Atrás modelo",16}"
                        + $"{"Adelante pilas",16}{"Adelante modelo",18}";
        sb.AppendLine(header);

        var gcRows = new List<string>();
        foreach (int n in new[] { 100_000, 1_000_000 })
        {
            string[] urls = CreateUrls(n);
            var history = new BrowserHistory();
            var model = new ReferenceModel();

            Timing visitStacks = Measure(() => { for (int i = 0; i < n; i++) history.Visit(urls[i]); },
                                         () => history.Clear());
            Timing visitModel = Measure(() => { for (int i = 0; i < n; i++) model.Visit(urls[i]); },
                                        () => model.Clear());

            Timing backStacks = Measure(() => { for (int i = 0; i < n - 1; i++) history.Back(); },
                                        () => Prepare(history, urls, 0));
            Timing backModel = Measure(() => { for (int i = 0; i < n - 1; i++) model.Back(); },
                                       () => Prepare(model, urls, 0));

            Timing forwardStacks = Measure(() => { for (int i = 0; i < n - 1; i++) history.Forward(); },
                                           () => Prepare(history, urls, n - 1));
            Timing forwardModel = Measure(() => { for (int i = 0; i < n - 1; i++) model.Forward(); },
                                          () => Prepare(model, urls, n - 1));

            sb.AppendLine($"{n,12:N0}{Ns(visitStacks, n),16}{Ns(visitModel, n),16}"
                          + $"{Ns(backStacks, n - 1),16}{Ns(backModel, n - 1),16}"
                          + $"{Ns(forwardStacks, n - 1),16}{Ns(forwardModel, n - 1),18}");
            gcRows.Add($"{n,12:N0}{Gc(visitStacks),16}{Gc(visitModel),16}{Gc(backStacks),16}{Gc(backModel),16}"
                       + $"{Gc(forwardStacks),16}{Gc(forwardModel),18}");
        }
        sb.AppendLine();
        sb.AppendLine("Cada corrida hace n Visit, n - 1 Atrás o n - 1 Adelante (el estado previo se prepara fuera del cronómetro).");
        sb.AppendLine();

        sb.AppendLine("Recolecciones del GC durante la corrida medida (generación 0 / generación 2)");
        sb.AppendLine(header);
        foreach (string row in gcRows)
            sb.AppendLine(row);
    }

    // Visit right after going back: the stacks drop the whole "forward" stack with Clear (head and tail = null),
    // the model has to remove every page after the index.
    private static void AppendVisitThatClearsForward(StringBuilder sb)
    {
        sb.AppendLine("Visit después de retroceder: descartar las páginas de adelante (una sola operación, en µs)");
        sb.AppendLine("Antes de cada corrida: n visitas y n - 1 retrocesos, así que adelante tiene n - 1 páginas.");
        sb.AppendLine($"{"n",12}{"Pilas (µs)",16}{"Modelo (µs)",16}");

        const string extra = "https://nueva-pagina.com";
        foreach (int n in new[] { 100_000, 1_000_000 })
        {
            string[] urls = CreateUrls(n);
            var history = new BrowserHistory();
            var model = new ReferenceModel();

            Timing stacks = Measure(() => history.Visit(extra), () => Prepare(history, urls, n - 1));
            Timing reference = Measure(() => model.Visit(extra), () => Prepare(model, urls, n - 1));

            sb.AppendLine($"{n,12:N0}{Micro(stacks),16}{Micro(reference),16}");
        }
        sb.AppendLine("Clear de la pila solo suelta head y tail; los nodos los libera después el recolector y ese costo no entra aquí.");
    }

    private static string[] CreateUrls(int n)
    {
        var urls = new string[n];
        for (int i = 0; i < n; i++)
            urls[i] = $"https://sitio{i}.com";
        return urls;
    }

    // Empty history, then visit every page and go back backSteps times.
    private static void Prepare(BrowserHistory history, string[] urls, int backSteps)
    {
        history.Clear();
        foreach (string url in urls)
            history.Visit(url);
        for (int i = 0; i < backSteps; i++)
            history.Back();
    }

    private static void Prepare(ReferenceModel model, string[] urls, int backSteps)
    {
        model.Clear();
        foreach (string url in urls)
            model.Visit(url);
        for (int i = 0; i < backSteps; i++)
            model.Back();
    }

    private readonly record struct Timing(double Ms, int Gen0, int Gen2);

    // Median of 5 timed runs after 2 warm-up runs (the JIT tiers up the code during the first calls).
    // setup runs before each run and is not measured. Gen0/Gen2 are the collections during the median run.
    private static Timing Measure(Action action, Action? setup = null)
    {
        for (int w = 0; w < 2; w++)
        {
            setup?.Invoke();
            action();
        }

        var runs = new Timing[5];
        for (int k = 0; k < runs.Length; k++)
        {
            setup?.Invoke();
            int gen0 = GC.CollectionCount(0);
            int gen2 = GC.CollectionCount(2);
            var watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            runs[k] = new Timing(watch.Elapsed.TotalMilliseconds, GC.CollectionCount(0) - gen0, GC.CollectionCount(2) - gen2);
        }
        Array.Sort(runs, (x, y) => x.Ms.CompareTo(y.Ms));
        return runs[2];
    }

    // Nanoseconds per operation for a run that did "operations" operations.
    private static string Ns(Timing timing, int operations) =>
        (timing.Ms * 1_000_000 / operations).ToString("F2", CultureInfo.InvariantCulture);

    private static string Micro(Timing timing) =>
        (timing.Ms * 1000).ToString("F3", CultureInfo.InvariantCulture);

    private static string Gc(Timing timing) => $"{timing.Gen0} / {timing.Gen2}";
}
