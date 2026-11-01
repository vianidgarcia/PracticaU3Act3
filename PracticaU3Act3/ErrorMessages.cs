namespace PracticaU3Act3;

// Messages shown to the user. The code is in English, but the messages are in Spanish.
public static class ErrorMessages
{
    // Used by SinglyLinkedList and DynamicStack.
    public const string EmptyList = "La lista está vacía.";
    public const string EmptyStack = "La pila está vacía: no hay elemento que sacar ni consultar.";

    public static string InvalidIndex(int index, int count) =>
        $"El índice {index} está fuera de rango. La lista tiene {count} elemento(s).";

    // Used by BrowserHistory and UrlNormalizer.
    public const string NoPreviousPage = "No hay una página anterior: no se puede retroceder.";
    public const string NoNextPage = "No hay una página siguiente: no se puede avanzar.";
    public const string EmptyAddress = "Escribe una dirección.";
    public const string AddressWithSpaces = "La dirección no puede contener espacios.";
}
