namespace PracticaU3Act3;

// Stack (LIFO) on top of the singly linked list: the top of the stack is the head of the list,
// so Push and Pop only move the head reference and cost O(1). There is no IsFull: it grows until memory runs out.
public sealed class DynamicStack<T>
{
    private readonly SinglyLinkedList<T> _list = new();

    public int Count => _list.Count;

    public bool IsEmpty() => _list.IsEmpty();

    public void Push(T value) => _list.InsertFirst(value);

    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException(ErrorMessages.EmptyStack);
        return _list.RemoveFirst();
    }

    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException(ErrorMessages.EmptyStack);
        return _list.PeekFirst();
    }

    public void Clear() => _list.Clear();

    // Top first.
    public T[] ToArray() => _list.ToArray();
}
