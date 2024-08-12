using System;
using System.Collections.Generic;

namespace DList;

internal record Node<T>
{
    public T Value { get; set; }
    public Node<T> Prev { get; set; }
    public Node<T> Next { get; set; }

    public Node(T value)
    {
        Value = value;
    }
}

internal record List<T>
{
    private int size;
    private Node<T> head;
    private Node<T> tail;

    public List()
    {
        size = 0;
        head = null;
        tail = null;
    }

    public int Size()
    {
        return size;
    }

    public void PushFront(T value)
    {
        var newNode = new Node<T>(value) { Next = head };

        if (head == null)
        {
            tail = newNode;
        }
        else
        {
            head.Prev = newNode;
        }

        head = newNode;
        size++;
    }

    public void PushBack(T value)
    {
        var newNode = new Node<T>(value) { Prev = tail };

        if (tail == null)
        {
            head = newNode;
        }
        else
        {
            tail.Next = newNode;
        }

        tail = newNode;
        size++;
    }

    public void PushAllFront(params T[] values)
    {
        foreach (var value in values)
        {
            PushFront(value);
        }
    }

    public void PushAllBack(params T[] values)
    {
        foreach (var value in values)
        {
            PushBack(value);
        }
    }

    public (T, bool) PopFront()
    {
        if (size == 0)
        {
            return (default(T), false);
        }

        var popped = head;
        head = head.Next;

        if (head == null)
        {
            tail = null;
        }
        else
        {
            head.Prev = null;
        }

        size--;
        return (popped.Value, true);
    }

    public (T, bool) PopBack()
    {
        if (size == 0)
        {
            return (default(T), false);
        }

        var popped = tail;
        tail = tail.Prev;

        if (tail == null)
        {
            head = null;
        }
        else
        {
            tail.Next = null;
        }

        size--;
        return (popped.Value, true);
    }

    public T ValueAt(int i)
    {
        return GetNodeAt(i).Value;
    }

    public void InsertAt(int i, T value)
    {
        var n = GetNodeAt(i);
        var newNode = new Node<T>(value) { Prev = n.Prev, Next = n };

        if (n.Prev == null)
        {
            head = newNode;
        }
        else
        {
            n.Prev.Next = newNode;
        }

        n.Prev = newNode;
        size++;
    }

    public void DeleteAt(int i)
    {
        throw new NotImplementedException("DIY...");
    }

    private Node<T> GetNodeAt(int i)
    {
        if (i < 0 || i >= size)
        {
            throw new IndexOutOfRangeException("index out of range");
        }

        return i < (size - 1) / 2 ? TraverseForwards(i) : TraverseBackwards(i);
    }

    private Node<T> TraverseForwards(int i)
    {
        var curr = head;

        for (var c = 0; c < i; c++)
        {
            curr = curr.Next;
        }

        return curr;
    }

    private Node<T> TraverseBackwards(int i)
    {
        var curr = tail;

        for (var c = size - 1; c > i; c--)
        {
            curr = curr.Prev;
        }

        return curr;
    }

    public IIterator<T> Iterator()
    {
        return new Iter<T>(head);
    }

    public IIterator<T> ReverseIterator()
    {
        return new ReverseIter<T>(tail);
    }
}

internal interface IIterator<T>
{
    bool HasNext();
    T Next();
}

internal class Iter<T> : IIterator<T>
{
    private Node<T> current;

    public Iter(Node<T> start)
    {
        current = start;
    }

    public bool HasNext()
    {
        return current != null;
    }

    public T Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("end of iteration");
        }

        var val = current.Value;
        current = current.Next;
        return val;
    }
}

internal class ReverseIter<T> : IIterator<T>
{
    private Node<T> current;

    public ReverseIter(Node<T> start)
    {
        current = start;
    }

    public bool HasNext()
    {
        return current != null;
    }

    public T Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("end of iteration");
        }

        var val = current.Value;
        current = current.Prev;
        return val;
    }
}
