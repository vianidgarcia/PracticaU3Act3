namespace PracticaU3Act3;

// Cleans up what the user types in the address bar. It does NOT check that the site exists:
// this is a simulation, so any text without spaces is accepted as an address.
public static class UrlNormalizer
{
    private const string HttpPrefix = "http://";
    private const string HttpsPrefix = "https://";

    // Returns true and the normalized address, or false and a message in Spanish.
    // Rules: trim the ends, reject empty text or text with inner spaces,
    // and add "https://" when the address does not start with "http://" or "https://".
    public static bool TryNormalize(string? input, out string normalized, out string error)
    {
        normalized = string.Empty;
        error = string.Empty;

        string text = (input ?? string.Empty).Trim();
        if (text.Length == 0)
        {
            error = ErrorMessages.EmptyAddress;
            return false;
        }

        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c))
            {
                error = ErrorMessages.AddressWithSpaces;
                return false;
            }
        }

        int prefixLength = ExistingPrefixLength(text);

        // Only "http://" or "https://" with nothing after it.
        if (prefixLength == text.Length)
        {
            error = ErrorMessages.EmptyAddress;
            return false;
        }

        normalized = prefixLength > 0 ? text : HttpsPrefix + text;
        return true;
    }

    // Length of the "http://" or "https://" prefix (any letter case), or 0 if the text has neither.
    private static int ExistingPrefixLength(string text)
    {
        if (text.StartsWith(HttpsPrefix, StringComparison.OrdinalIgnoreCase))
            return HttpsPrefix.Length;
        if (text.StartsWith(HttpPrefix, StringComparison.OrdinalIgnoreCase))
            return HttpPrefix.Length;
        return 0;
    }
}
