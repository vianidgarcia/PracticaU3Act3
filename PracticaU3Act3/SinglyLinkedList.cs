using System.Text;

namespace PracticaU3Act3;

// Singly linked list built from scratch with dynamic nodes.
// It keeps a reference to the first node (_head), a reference to the last one (_tail)
// and a counter, so inserting at the ends and Count are O(1).
public sealed class SinglyLinkedList<T>
{
    // The node is private: nobody outside the list can touch the links.
    private sealed class Node
    {
        public T Value;
        public Node? Next;

        public Node(T value, Node? next = null)
        {
            Value = value;
            Next = next;
        }
    }

    private Node? _head;
    private Node? _tail;
    private readonly IComparer<T> _comparer;

    public SinglyLinkedList(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    public int Count { get; private set; }

    public bool IsEmpty() => Count == 0;

    // ---------- Insertion ----------

    public void InsertFirst(T value)
    {
        _head = new Node(value, _head);
        _tail ??= _head;
        Count++;
    }

    public void InsertLast(T value)
    {
        var node = new Node(value);
        if (_tail == null)
        {
            _head = node;
        }
        else
        {
            _tail.Next = node;
        }
        _tail = node;
        Count++;
    }

    // index 0 inserts at the beginning; index == Count inserts at the end.
    public void InsertAt(int index, T value)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(null, ErrorMessages.InvalidIndex(index, Count));

        if (index == 0)
        {
            InsertFirst(value);
        }
        else if (index == Count)
        {
            InsertLast(value);
        }
        else
        {
            Node previous = NodeAt(index - 1);
            previous.Next = new Node(value, previous.Next);
            Count++;
        }
    }

    // Plain Add appends at the end.
    public void Add(T value) => InsertLast(value);

    // Sorted insertion in a single pass. Returns false (and changes nothing) if the value already exists.
    // Only meaningful when every element was inserted with AddSorted.
    public bool AddSorted(T value)
    {
        Node? previous = null;
        Node? current = _head;
        while (current != null && _comparer.Compare(current.Value, value) < 0)
        {
            previous = current;
            current = current.Next;
        }

        if (current != null && _comparer.Compare(current.Value, value) == 0)
            return false;

        var node = new Node(value, current);
        if (previous == null)
            _head = node;
        else
            previous.Next = node;

        if (current == null)
            _tail = node;

        Count++;
        return true;
    }

    // ---------- Removal ----------

    public T RemoveFirst()
    {
        if (_head == null)
            throw new InvalidOperationException(ErrorMessages.EmptyList);

        T value = _head.Value;
        _head = _head.Next;
        if (_head == null)
            _tail = null;
        Count--;
        return value;
    }

    // O(n): in a singly linked list the node before the tail can only be found by walking.
    public T RemoveLast()
    {
        if (_head == null || _tail == null)
            throw new InvalidOperationException(ErrorMessages.EmptyList);

        if (_head == _tail)
            return RemoveFirst();

        Node previous = _head;
        while (previous.Next != _tail)
            previous = previous.Next!;

        T value = _tail.Value;
        previous.Next = null;
        _tail = previous;
        Count--;
        return value;
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(null, ErrorMessages.InvalidIndex(index, Count));

        if (index == 0)
            return RemoveFirst();

        Node previous = NodeAt(index - 1);
        Node removed = previous.Next!;
        previous.Next = removed.Next;
        if (removed == _tail)
            _tail = previous;
        Count--;
        return removed.Value;
    }

    // Removes the first element equal to value. Returns false if it is not in the list.
    public bool Remove(T value)
    {
        var equality = EqualityComparer<T>.Default;
        Node? previous = null;
        Node? current = _head;
        while (current != null && !equality.Equals(current.Value, value))
        {
            previous = current;
            current = current.Next;
        }

        if (current == null)
            return false;

        if (previous == null)
            _head = current.Next;
        else
            previous.Next = current.Next;

        if (current == _tail)
            _tail = previous;

        Count--;
        return true;
    }

    public void Clear()
    {
        _head = null;
        _tail = null;
        Count = 0;
    }

    // ---------- Query ----------

    // Index of the first element equal to value, or -1.
    public int Search(T value)
    {
        var equality = EqualityComparer<T>.Default;
        int index = 0;
        for (Node? current = _head; current != null; current = current.Next, index++)
        {
            if (equality.Equals(current.Value, value))
                return index;
        }
        return -1;
    }

    public bool Exist(T value) => Search(value) >= 0;

    public T Get(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(null, ErrorMessages.InvalidIndex(index, Count));
        return NodeAt(index).Value;
    }

    public T PeekFirst()
    {
        if (_head == null)
            throw new InvalidOperationException(ErrorMessages.EmptyList);
        return _head.Value;
    }

    public T PeekLast()
    {
        if (_tail == null)
            throw new InvalidOperationException(ErrorMessages.EmptyList);
        return _tail.Value;
    }

    // Replaces the first element equal to oldValue with newValue. Returns false if it is not found.
    public bool Edit(T oldValue, T newValue)
    {
        var equality = EqualityComparer<T>.Default;
        for (Node? current = _head; current != null; current = current.Next)
        {
            if (equality.Equals(current.Value, oldValue))
            {
                current.Value = newValue;
                return true;
            }
        }
        return false;
    }

    // Read-only copy of the values, used to draw the list without exposing the nodes.
    public T[] ToArray()
    {
        var result = new T[Count];
        int i = 0;
        for (Node? current = _head; current != null; current = current.Next)
            result[i++] = current.Value;
        return result;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (Node? current = _head; current != null; current = current.Next)
        {
            if (sb.Length > 0)
                sb.Append(" -> ");
            sb.Append(current.Value);
        }
        return sb.ToString();
    }

    private Node NodeAt(int index)
    {
        Node current = _head!;
        for (int i = 0; i < index; i++)
            current = current.Next!;
        return current;
    }
}