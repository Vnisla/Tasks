using System;
using System.Collections.Generic;
//Задача 5
public class MyHeap<T>
{
    private List<T> elements;
    private IComparer<T> comparer;
    private bool isMaxHeap;
    public MyHeap(bool isMaxHeap = true) : this(isMaxHeap, null) { }
    public MyHeap(bool isMaxHeap, IComparer<T> comparer)
    {
        this.elements = new List<T>();
        this.isMaxHeap = isMaxHeap;
        this.comparer = comparer ?? Comparer<T>.Default;
    }
    public MyHeap(T[] array, bool isMaxHeap = true) : this(array, isMaxHeap, null) { }
    public MyHeap(T[] array, bool isMaxHeap, IComparer<T> comparer) : this(isMaxHeap, comparer)
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
    public MyHeap<T> Merge(MyHeap<T> other)
    {
        if (this.isMaxHeap != other.isMaxHeap)
            throw new InvalidOperationException("Cannot merge heaps of different types");
        var mergedHeap = new MyHeap<T>(this.isMaxHeap, this.comparer);
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
//Задача 6
public class MyPriorityQueue<T>
{
    private MyHeap<T> heap;
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
        this.heap = new MyHeap<T>(true, comparator); 
        this.elements = new List<T>();
    }
    public MyPriorityQueue(MyPriorityQueue<T> other)
    {
        this.heap = new MyHeap<T>(true, Comparer<T>.Default);
        this.elements = new List<T>(other.elements);
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
        heap = new MyHeap<T>(true, Comparer<T>.Default);
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
        var newHeap = new MyHeap<T>(true, Comparer<T>.Default);
        foreach (var item in elements)
        {
            newHeap.Add(item);
        }
        heap = newHeap;
    }
    public void PrintHeapStructure()
    {
        var heapElements = heap.GetElements();
        if (heapElements.Count == 0)
        {
            Console.WriteLine("Куча пуста");
            return;
        }
        int level = 0;
        int itemsInLevel = 1;
        int count = 0;
        foreach (var element in heapElements)
        {
            if (count % itemsInLevel == 0)
            {
                Console.WriteLine();
                Console.Write($"Уровень {level}: ");
                itemsInLevel *= 2;
                level++;
            }
            Console.Write($"{element} ");
            count++;
        }
        Console.WriteLine();
    }
}
//Класс заявки
public class Request : IComparable<Request>
{
    public int Priority { get; set; }
    public int RequestId { get; set; }
    public int StepAdded { get; set; }
    public int? StepRemoved { get; set; }
    public int WaitTime => (StepRemoved ?? 0) - StepAdded;
    public Request(int priority, int requestId, int stepAdded)
    {
        Priority = priority;
        RequestId = requestId;
        StepAdded = stepAdded;
        StepRemoved = null;
    }
    public int CompareTo(Request other)
    {
        int priorityCompare = other.Priority.CompareTo(this.Priority);
        if (priorityCompare != 0)
            return priorityCompare;
        return this.StepAdded.CompareTo(other.StepAdded);
    }
    public override string ToString()
    {
        return $"Заявка {RequestId} (приоритет: {Priority}, шаг {StepAdded}-{StepRemoved}, ожидание: {WaitTime})";
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine(" системы обработки заявок");
        Console.WriteLine("MyHeap и MyPriorityQueue");
        Console.Write("Введите количество шагов добавления заявок (N): ");
        if (!int.TryParse(Console.ReadLine(), out int N) || N <= 0)
        {
            Console.WriteLine("Ошибка: введите положительное целое число");
            return;
        }
        var queue = new MyPriorityQueue<Request>();
        var random = new Random();
        int requestCounter = 1;
        Request maxWaitRequest = null;
        int totalRequestsProcessed = 0;
        File.WriteAllText("log.txt", "");
        Console.WriteLine($"\nЗапуск симуляции на {N} шагов");
        for (int step = 1; step <= N; step++)
        {
            Console.WriteLine($"\n Шаг {step} ");
            int requestsToAdd = random.Next(1, 11);
            Console.WriteLine($"Добавляется заявок: {requestsToAdd}");
            for (int i = 0; i < requestsToAdd; i++)
            {
                int priority = random.Next(1, 6);
                var request = new Request(priority, requestCounter, step);
                queue.Add(request);
                LogToFile("ADD", request.RequestId, request.Priority, step);
                Console.WriteLine($"  Добавлена: {request}");
                requestCounter++;
            }
            if (!queue.IsEmpty())
            {
                var removedRequest = queue.Poll();
                removedRequest.StepRemoved = step;
                totalRequestsProcessed++;
                LogToFile("REMOVE", removedRequest.RequestId, removedRequest.Priority, step);
                Console.WriteLine($"  Удалена: {removedRequest}");
                if (maxWaitRequest == null || removedRequest.WaitTime > maxWaitRequest.WaitTime)
                {
                    maxWaitRequest = removedRequest;
                }
            }
            else
            {
                Console.WriteLine("  Очередь пуста - нечего удалять");
            }

            Console.WriteLine($"Текущий размер очереди: {queue.Size()}");
        }
        Console.WriteLine($"\nЗавершена фаза добавления. Осталось заявок: {queue.Size()}");
        Console.WriteLine("Переходим к фазе очистки");
        int cleanupStep = N + 1;
        while (!queue.IsEmpty())
        {
            Console.WriteLine($"\nШаг очистки {cleanupStep} ");
            var removedRequest = queue.Poll();
            removedRequest.StepRemoved = cleanupStep;
            totalRequestsProcessed++;
            LogToFile("REMOVE", removedRequest.RequestId, removedRequest.Priority, cleanupStep);
            Console.WriteLine($"  Удалена: {removedRequest}");
            if (maxWaitRequest == null || removedRequest.WaitTime > maxWaitRequest.WaitTime)
            {
                maxWaitRequest = removedRequest;
            }
            Console.WriteLine($"Осталось заявок: {queue.Size()}");
            cleanupStep++;
        }
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("РЕЗУЛЬТАТЫ");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"Всего сгенерировано заявок: {requestCounter - 1}");
        Console.WriteLine($"Всего обработано заявок: {totalRequestsProcessed}");
        if (maxWaitRequest != null)
        {
            Console.WriteLine($"\nЗаявка с максимальным временем ожидания:");
            Console.WriteLine($"  Номер: {maxWaitRequest.RequestId}");
            Console.WriteLine($"  Приоритет: {maxWaitRequest.Priority}");
            Console.WriteLine($"  Добавлена на шаге: {maxWaitRequest.StepAdded}");
            Console.WriteLine($"  Удалена на шаге: {maxWaitRequest.StepRemoved}");
            Console.WriteLine($"  Время ожидания: {maxWaitRequest.WaitTime} шагов");
        }
        Console.WriteLine($"\nЛог операций сохранен в файл: log.txt");
    }
    static void LogToFile(string action, int requestId, int priority, int step)
    {
        string logEntry = $"{action} {requestId} {priority} {step}";
        File.AppendAllText("log.txt", logEntry + Environment.NewLine);
    }
}
