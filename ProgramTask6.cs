using System;
using System.Collections.Generic;
public class Heap<T>
{
    private List<T> elements;
    private IComparer<T> comparer;
    private bool isMaxHeap;
    public Heap(bool isMaxHeap = true) : this(isMaxHeap, null) { }
    public Heap(bool isMaxHeap, IComparer<T> comparer)
    {
        this.elements = new List<T>();
        this.isMaxHeap = isMaxHeap;
        this.comparer = comparer ?? Comparer<T>.Default;
    }
    public Heap(T[] array, bool isMaxHeap = true) : this(array, isMaxHeap, null) { }
    public Heap(T[] array, bool isMaxHeap, IComparer<T> comparer) : this(isMaxHeap, comparer)
    {
        foreach (var item in array)
        {
            Add(item);
        }
    }
    public T Peek()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Heap is empty");
        return elements[0];
    }
    public T ExtractRoot()
    {
        if (elements.Count == 0)
            throw new InvalidOperationException("Heap is empty");
        T root = elements[0];
        elements[0] = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        HeapifyDown(0);
        return root;
    }
    public void Add(T item)
    {
        elements.Add(item);
        HeapifyUp(elements.Count - 1);
    }
    public void UpdateKey(int index, T newValue)
    {
        if (index < 0 || index >= elements.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        int compareResult = comparer.Compare(newValue, elements[index]);
        elements[index] = newValue;
        if (isMaxHeap ? compareResult > 0 : compareResult < 0)
            HeapifyUp(index);
        else
            HeapifyDown(index);
    }
    public Heap<T> Merge(Heap<T> other)
    {
        if (this.isMaxHeap != other.isMaxHeap)
            throw new InvalidOperationException("Cannot merge heaps of different types");
        var mergedHeap = new Heap<T>(this.isMaxHeap, this.comparer);
        foreach (var item in this.elements)
        {
            mergedHeap.Add(item);
        }
        foreach (var item in other.elements)
        {
            mergedHeap.Add(item);
        }
        return mergedHeap;
    }
    public int Count => elements.Count;
    public bool IsEmpty => elements.Count == 0;
    public List<T> GetElements() => new List<T>(elements);
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (ShouldSwap(parent, index))
                break;
            Swap(index, parent);
            index = parent;
        }
    }
    private void HeapifyDown(int index)
    {
        int lastIndex = elements.Count - 1;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int target = index;
            if (left <= lastIndex && ShouldSwap(target, left))
                target = left;
            if (right <= lastIndex && ShouldSwap(target, right))
                target = right;
            if (target == index)
                break;
            Swap(index, target);
            index = target;
        }
    }
    private bool ShouldSwap(int parent, int child)
    {
        int compareResult = comparer.Compare(elements[parent], elements[child]);
        return isMaxHeap ? compareResult >= 0 : compareResult <= 0;
    }
    private void Swap(int i, int j)
    {
        T temp = elements[i];
        elements[i] = elements[j];
        elements[j] = temp;
    }
}
// Задача 6
public class MyPriorityQueue<T>
{
    private Heap<T> heap;
    private List<T> elements; 
    public MyPriorityQueue() : this(Comparer<T>.Default) { }
    public MyPriorityQueue(T[] a) : this(Comparer<T>.Default)
    {
        AddAll(a);
    }
    public MyPriorityQueue(int initialCapacity) : this(Comparer<T>.Default) { }
    public MyPriorityQueue(int initialCapacity, IComparer<T> comparator) : this(comparator) { }
    public MyPriorityQueue(IComparer<T> comparator)
    {
        this.heap = new Heap<T>(true, comparator);
        this.elements = new List<T>();
    }
    public MyPriorityQueue(MyPriorityQueue<T> other)
    {
        this.heap = new Heap<T>(true, GetComparerFromHeap(other.heap));
        this.elements = new List<T>(other.elements);
    }
    private IComparer<T> GetComparerFromHeap(Heap<T> heapRef)
    {
        return Comparer<T>.Default;
    }
    public void Add(T e)
    {
        heap.Add(e);
        elements.Add(e);
    }
    public void AddAll(T[] a)
    {
        foreach (var item in a)
        {
            Add(item);
        }
    }
    public void Clear()
    {
        heap = new Heap<T>(true, GetComparerFromHeap(heap));
        elements.Clear();
    }
    public bool Contains(object o)
    {
        if (o is T item)
        {
            return elements.Contains(item);
        }
        return false;
    }
    public bool ContainsAll(T[] a)
    {
        foreach (var item in a)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }
    public bool IsEmpty()
    {
        return heap.IsEmpty;
    }
    public bool Remove(object o)
    {
        if (o is T item && elements.Remove(item))
        {
            RebuildHeapFromElements();
            return true;
        }
        return false;
    }
    public void RemoveAll(T[] a)
    {
        bool removed = false;
        foreach (var item in a)
        {
            if (elements.Remove(item))
                removed = true;
        }
        if (removed)
            RebuildHeapFromElements();
    }
    public void RetainAll(T[] a)
    {
        var toRetain = new HashSet<T>(a);
        elements.RemoveAll(item => !toRetain.Contains(item));
        RebuildHeapFromElements();
    }
    public int Size()
    {
        return elements.Count;
    }
    public T[] ToArray()
    {
        return elements.ToArray();
    }
    public T[] ToArray(T[] a)
    {
        if (a == null || a.Length < elements.Count)
            return ToArray();
        elements.CopyTo(a, 0);
        if (a.Length > elements.Count)
            a[elements.Count] = default(T);
        return a;
    }
    public T Element()
    {
        if (heap.IsEmpty)
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
        if (heap.IsEmpty)
            return default(T);
        return heap.Peek();
    }
    public T Poll()
    {
        if (heap.IsEmpty)
            return default(T);
        T item = heap.ExtractRoot();
        elements.Remove(item);
        return item;
    }
    private void RebuildHeapFromElements()
    {
        var newHeap = new Heap<T>(true, GetComparerFromHeap(heap));
        foreach (var item in elements)
        {
            newHeap.Add(item);
        }
        heap = newHeap;
    }
    public void PrintHeapStructure()
    {
        Console.WriteLine("Структура кучи:");
        var heapElements = heap.GetElements();
        for (int i = 0; i < heapElements.Count; i++)
        {
            Console.WriteLine($"  [{i}]: {heapElements[i]}");
        }
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine("MyPriorityQueue");
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  add <число> - добавить элемент");
        Console.WriteLine("  poll - извлечь элемент с высшим приоритетом");
        Console.WriteLine("  peek - посмотреть элемент с высшим приоритетом");
        Console.WriteLine("  size - показать размер очереди");
        Console.WriteLine("  show - показать все элементы");
        Console.WriteLine("  structure - показать структуру кучи");
        Console.WriteLine("  clear - очистить очередь");
        Console.WriteLine("  exit - выход");
        Console.WriteLine();
        var pq = new MyPriorityQueue<int>();
        while (true)
        {
            Console.Write("Введите команду: ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLower();
            try
            {
                switch (command)
                {
                    case "add":
                        if (parts.Length > 1 && int.TryParse(parts[1], out int num))
                        {
                            pq.Add(num);
                            Console.WriteLine($"Добавлен элемент: {num}");
                        }
                        break;

                    case "poll":
                        if (!pq.IsEmpty())
                        {
                            int max = pq.Poll();
                            Console.WriteLine($"Извлечен элемент с высшим приоритетом: {max}");
                        }
                        else
                        {
                            Console.WriteLine("Очередь пуста");
                        }
                        break;

                    case "peek":
                        if (!pq.IsEmpty())
                        {
                            int max = pq.Peek();
                            Console.WriteLine($"Элемент с высшим приоритетом: {max}");
                        }
                        else
                        {
                            Console.WriteLine("Очередь пуста");
                        }
                        break;

                    case "size":
                        Console.WriteLine($"Размер очереди: {pq.Size()}");
                        break;

                    case "show":
                        if (!pq.IsEmpty())
                        {
                            var arr = pq.ToArray();
                            Console.WriteLine("Все элементы: " + string.Join(", ", arr));
                        }
                        else
                        {
                            Console.WriteLine("Очередь пуста");
                        }
                        break;

                    case "structure":
                        pq.PrintHeapStructure();
                        break;
                    case "clear":
                        pq.Clear();
                        Console.WriteLine("Очередь очищена");
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы");
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            Console.WriteLine();
        }
    }
}
