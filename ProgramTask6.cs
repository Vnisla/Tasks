using System;
using System.Collections.Generic;
public class Heap<T> where T : IComparable<T>
{
    private List<T> elements;
    private bool isMinHeap;
    public Heap(bool minHeap = true)
    {
        elements = new List<T>();
        isMinHeap = minHeap;
    }
    public int Count => elements.Count;
    public bool IsEmpty => elements.Count == 0;
    private int Parent(int index) => (index - 1) / 2;
    private int LeftChild(int index) => 2 * index + 1;
    private int RightChild(int index) => 2 * index + 2;
    private bool ShouldSwap(T a, T b)
    {
        return isMinHeap ? a.CompareTo(b) < 0 : a.CompareTo(b) > 0;
    }
    public void Add(T item)
    {
        elements.Add(item);
        HeapifyUp(elements.Count - 1);
    }
    public T Peek()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty");
        return elements[0];
    }
    public T RemoveRoot()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty");
        T root = elements[0];
        elements[0] = elements[Count - 1];
        elements.RemoveAt(Count - 1);
        HeapifyDown(0);
        return root;
    }
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = Parent(index);
            if (ShouldSwap(elements[index], elements[parent]))
            {
                Swap(index, parent);
                index = parent;
            }
            else break;
        }
    }
    private void HeapifyDown(int index)
    {
        while (index < Count)
        {
            int left = LeftChild(index);
            int right = RightChild(index);
            int extreme = index;
            if (left < Count && ShouldSwap(elements[left], elements[extreme]))
                extreme = left;
            if (right < Count && ShouldSwap(elements[right], elements[extreme]))
                extreme = right;
            if (extreme != index)
            {
                Swap(index, extreme);
                index = extreme;
            }
            else break;
        }
    }
    private void Swap(int i, int j)
    {
        T temp = elements[i];
        elements[i] = elements[j];
        elements[j] = temp;
    }
    public bool Contains(T item) => elements.Contains(item);
    public List<T> CopyElements()
    {
        return new List<T>(elements);
    }
}
// zadacha 6
public class MyPriorityQueue<T> where T : IComparable<T>
{
    private Heap<T> heap;
    private int capacity;
    private int count;
    public MyPriorityQueue() : this(11) { }
    public MyPriorityQueue(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Capacity must be positive");
        capacity = initialCapacity;
        count = 0;
        heap = new Heap<T>(true);
    }
    public MyPriorityQueue(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        capacity = Math.Max(array.Length * 2, 11);
        heap = new Heap<T>(true);
        foreach (var item in array)
            Add(item);
    }
    public MyPriorityQueue(MyPriorityQueue<T> other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));
        capacity = other.capacity;
        count = other.count;
        heap = new Heap<T>(true);
        var elements = other.heap.CopyElements();
        foreach (var item in elements)
            heap.Add(item);
    }
    public int Count => count;
    public bool IsEmpty => count == 0;
    public void Add(T element)
    {
        EnsureCapacity();
        heap.Add(element);
        count++;
    }
    public void AddAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        foreach (var item in array)
            Add(item);
    }
    public void Clear()
    {
        heap = new Heap<T>(true);
        count = 0;
    }
    public bool Contains(object obj)
    {
        if (obj is T item)
            return heap.Contains(item);
        return false;
    }
    public bool ContainsAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        foreach (var item in array)
            if (!Contains(item))
                return false;
        return true;
    }
    public T Element()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty");
        return heap.Peek();
    }
    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public T Peek()
    {
        if (IsEmpty)
            return default(T);
        return heap.Peek();
    }
    public T Poll()
    {
        if (IsEmpty)
            return default(T);
        count--;
        return heap.RemoveRoot();
    }
    public bool Remove(object obj)
    {
        if (!(obj is T item))
            return false;
        Heap<T> newHeap = new Heap<T>(true);
        bool found = false;
        var elements = heap.CopyElements();
        foreach (var current in elements)
        {
            if (!found && current.Equals(item))
            {
                found = true;
                count--;
            }
            else
            {
                newHeap.Add(current);
            }
        }
        heap = newHeap;
        return found;
    }
    public void RemoveAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        foreach (var item in array)
            Remove(item);
    }
    public void RetainAll(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        var retainSet = new HashSet<T>(array);
        Heap<T> newHeap = new Heap<T>(true);
        var elements = heap.CopyElements();
        foreach (var current in elements)
        {
            if (retainSet.Contains(current))
            {
                newHeap.Add(current);
            }
            else
            {
                count--;
            }
        }
        heap = newHeap;
    }
    public int Size() => count;
    public T[] ToArray()
    {
        var tempHeap = new Heap<T>(true);
        var elements = heap.CopyElements();
        foreach (var item in elements)
            tempHeap.Add(item);
        var result = new List<T>();
        while (!tempHeap.IsEmpty)
            result.Add(tempHeap.RemoveRoot());
        return result.ToArray();
    }
    public T[] ToArray(T[] array)
    {
        T[] result = ToArray();
        if (array == null)
            return result;
        if (array.Length < result.Length)
            return result;
        Array.Copy(result, array, result.Length);
        for (int i = result.Length; i < array.Length; i++)
            array[i] = default(T);
        return array;
    }
    private void EnsureCapacity()
    {
        if (count >= capacity)
        {
            if (capacity < 64)
                capacity += 2;
            else
                capacity = (int)(capacity * 1.5);
        }
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine(" ОЧЕРЕДЬ С ПРИОРИТЕТАМИ");
        Console.WriteLine("\n Тест 1: Базa");
        MyPriorityQueue<int> queue = new MyPriorityQueue<int>();
        int[] elements = { 5, 3, 8, 1, 10, 2 };
        Console.WriteLine("Добавляем элементы: " + string.Join(", ", elements));
        queue.AddAll(elements);
        Console.WriteLine($"Размер: {queue.Size()}");
        Console.WriteLine($"Peek: {queue.Peek()}");
        Console.WriteLine("Содержимое в порядке приоритета: " + string.Join(" ", queue.ToArray()));
        Console.WriteLine("\n Тест 2: Извлечение элементов ");
        while (!queue.IsEmpty)
        {
            Console.WriteLine($"Poll: {queue.Poll()}, Осталось: {queue.Size()}");
        }
        Console.WriteLine("\n Тест 3: Операции toarray & contains");
        queue.AddAll(new int[] { 1, 2, 3, 4, 5 });
        Console.WriteLine("Содержимое: " + string.Join(" ", queue.ToArray()));
        Console.WriteLine($"Contains 3: {queue.Contains(3)}");
        Console.WriteLine($"Contains 10: {queue.Contains(10)}");
        queue.Remove(3);
        Console.WriteLine("После удаления 3: " + string.Join(" ", queue.ToArray()));
        Console.WriteLine("\n Тест 4: RetainAll");
        queue.RetainAll(new int[] { 1, 5 });
        Console.WriteLine("После RetainAll [1, 5]: " + string.Join(" ", queue.ToArray()));
        Console.WriteLine("\n Тест 5: Копирование ");
        MyPriorityQueue<int> copy = new MyPriorityQueue<int>(queue);
        Console.WriteLine("Копия: " + string.Join(" ", copy.ToArray()));
    }
}
