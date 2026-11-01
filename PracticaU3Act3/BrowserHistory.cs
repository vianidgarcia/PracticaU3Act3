namespace PracticaU3Act3;

// Back / Forward history of a browser, built with two stacks.
//   _back    : pages visited before the current one (top = the page you return to with Back).
//   _forward : pages you left with Back (top = the page you return to with Forward).
// Back and Forward only move one element between the stacks, so every operation is O(1).
// This class knows nothing about the user interface.
public sealed class BrowserHistory
{
    private readonly DynamicStack<string> _back = new();
    private readonly DynamicStack<string> _forward = new();

    // Null until the first visit.
    public string? Current { get; private set; }

    public int BackCount => _back.Count;
    public int ForwardCount => _forward.Count;
    public bool CanGoBack => !_back.IsEmpty();
    public bool CanGoForward => !_forward.IsEmpty();

    // Goes to a new page. Returns false (and changes nothing) if it is the page already shown, like a reload.
    // Throws ArgumentException (message in Spanish) if the address is not valid.
    public bool Visit(string url)
    {
        if (!UrlNormalizer.TryNormalize(url, out string address, out string error))
            throw new ArgumentException(error);

        if (Current != null && string.Equals(Current, address, StringComparison.Ordinal))
            return false;

        if (Current != null)
            _back.Push(Current);

        Current = address;

        // A new visit cancels the "future": Clear drops the references in O(1).
        _forward.Clear();
        return true;
    }

    // Returns to the previous page and keeps the one we left so Forward can bring it back.
    public string Back()
    {
        if (Current == null || _back.IsEmpty())
            throw new InvalidOperationException(ErrorMessages.NoPreviousPage);

        _forward.Push(Current);
        Current = _back.Pop();
        return Current;
    }

    // Symmetric to Back.
    public string Forward()
    {
        if (Current == null || _forward.IsEmpty())
            throw new InvalidOperationException(ErrorMessages.NoNextPage);

        _back.Push(Current);
        Current = _forward.Pop();
        return Current;
    }

    public void Clear()
    {
        _back.Clear();
        _forward.Clear();
        Current = null;
    }

    // Copies of the stacks from top to bottom, used to draw them and to check them in the tests.
    public string[] BackItems() => _back.ToArray();
    public string[] ForwardItems() => _forward.ToArray();
}
