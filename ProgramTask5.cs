using System;
using System.Collections.Generic;
public class Heap<T>
{
    private List<T> _elements;
    private readonly IComparer<T> _comparer;
    public Heap() : this(Comparer<T>.Default) { }
    public Heap(IComparer<T> comparer)
    {
        _elements = new List<T>();
        _comparer = comparer ?? Comparer<T>.Default;
    }
    public Heap(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }
    public Heap(IEnumerable<T> collection, IComparer<T> comparer)
    {
        _elements = new List<T>(collection);
        _comparer = comparer ?? Comparer<T>.Default;
        for (int i = _elements.Count / 2 - 1; i >= 0; i--)
        {
            HeapifyDown(i);
        }
    }
    public int Count => _elements.Count;
    public bool IsEmpty => _elements.Count == 0;
    public T Peek()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Куча пуста");
        return _elements[0];
    }
    public T ExtractRoot()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Куча пуста");
        T root = _elements[0];
        int lastIndex = _elements.Count - 1;
        _elements[0] = _elements[lastIndex];
        _elements.RemoveAt(lastIndex);
        if (_elements.Count > 0)
        {
            HeapifyDown(0);
        }

        return root;
    }
    public void Add(T item)
    {
        _elements.Add(item);
        HeapifyUp(_elements.Count - 1);
    }
    public void UpdateKey(int index, T newValue)
    {
        if (index < 0 || index >= _elements.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = _elements[index];
        _elements[index] = newValue;
        int comparison = _comparer.Compare(newValue, oldValue);
        if (comparison > 0)
        {
            HeapifyUp(index);
        }
        else if (comparison < 0)
        {
            HeapifyDown(index);
        }
    }
    public void Merge(Heap<T> otherHeap)
    {
        if (otherHeap == null)
            throw new ArgumentNullException(nameof(otherHeap));
        foreach (T item in otherHeap._elements)
        {
            Add(item);
        }
    }
    public bool Contains(T item)
    {
        return _elements.Contains(item);
    }
    public void Clear()
    {
        _elements.Clear();
    }
    public T[] ToArray()
    {
        return _elements.ToArray();
    }
    public void Print()
    {
        Console.WriteLine("Элементы кучи: " + string.Join(", ", _elements));
    }
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (_comparer.Compare(_elements[index], _elements[parentIndex]) <= 0)
                break;
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }
    private void HeapifyDown(int index)
    {
        int lastIndex = _elements.Count - 1;
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;
            int largestIndex = index;
            if (leftChildIndex <= lastIndex &&
                _comparer.Compare(_elements[leftChildIndex], _elements[largestIndex]) > 0)
            {
                largestIndex = leftChildIndex;
            }
            if (rightChildIndex <= lastIndex &&
                _comparer.Compare(_elements[rightChildIndex], _elements[largestIndex]) > 0)
            {
                largestIndex = rightChildIndex;
            }
            if (largestIndex == index)
                break;
            Swap(index, largestIndex);
            index = largestIndex;
        }
    }
    private void Swap(int i, int j)
    {
        T temp = _elements[i];
        _elements[i] = _elements[j];
        _elements[j] = temp;
    }
}
public class MinHeap<T> : Heap<T>
{
    public MinHeap() : base(new ReverseComparer<T>(Comparer<T>.Default)) { }
    public MinHeap(IComparer<T> comparer) : base(new ReverseComparer<T>(comparer)) { }
    public MinHeap(IEnumerable<T> collection) : base(collection, new ReverseComparer<T>(Comparer<T>.Default)) { }
    public MinHeap(IEnumerable<T> collection, IComparer<T> comparer) : base(collection, new ReverseComparer<T>(comparer)) { }
}
public class ReverseComparer<T> : IComparer<T>
{
    private readonly IComparer<T> _originalComparer;
    public ReverseComparer(IComparer<T> originalComparer)
    {
        _originalComparer = originalComparer ?? Comparer<T>.Default;
    }
    public int Compare(T x, T y)
    {
        return _originalComparer.Compare(y, x);
    }
}
public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Максимальная Куча ===");
        DemoMaxHeap();
        Console.WriteLine("\n=== Минимальной Куча ===");
        DemoMinHeap();
        Console.WriteLine("\n=== Создание кучи из массива ===");
        DemoHeapFromArray();
        Console.WriteLine("\n=== Слияние куч ===");
        DemoHeapMerge();
        Console.WriteLine("\n=== Обновление ключа ===");
        DemoUpdateKey();
    }
    static void DemoMaxHeap()
    {
        var maxHeap = new Heap<int>();

        maxHeap.Add(10);
        maxHeap.Add(30);
        maxHeap.Add(20);
        maxHeap.Add(5);
        maxHeap.Add(25);
        maxHeap.Print();
        Console.WriteLine($"Верхний элемент: {maxHeap.Peek()}");
        Console.WriteLine($"Извлеченный элемент: {maxHeap.ExtractRoot()}");
        Console.Write("После извлечения: ");
        maxHeap.Print();
    }
    static void DemoMinHeap()
    {
        var minHeap = new MinHeap<int>();
        minHeap.Add(10);
        minHeap.Add(30);
        minHeap.Add(20);
        minHeap.Add(5);
        minHeap.Add(25);
        minHeap.Print();
        Console.WriteLine($"Верхний элемент: {minHeap.Peek()}");
        Console.WriteLine($"Извлеченный элемент: {minHeap.ExtractRoot()}");
        Console.Write("После извлечения: ");
        minHeap.Print();
    }
    static void DemoHeapFromArray()
    {
        int[] array = { 15, 8, 22, 3, 17, 9 };
        var heapFromArray = new Heap<int>(array);
        Console.Write("Куча созданная из массива: ");
        heapFromArray.Print();
    }
    static void DemoHeapMerge()
    {
        var heap1 = new Heap<int>(new[] { 10, 20, 30 });
        var heap2 = new Heap<int>(new[] { 5, 25, 15 });
        heap1.Merge(heap2);
        Console.Write("Объединенная куча: ");
        heap1.Print();
    }
    static void DemoUpdateKey()
    {
        var heap = new Heap<int>(new[] { 10, 20, 30, 40, 50 });
        Console.Write("До обновления: ");
        heap.Print();
        heap.UpdateKey(3, 60);
        Console.Write("После обновления: ");
        heap.Print();
    }
}
public class StringLengthComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        return x.Length.CompareTo(y.Length);
    }
}
