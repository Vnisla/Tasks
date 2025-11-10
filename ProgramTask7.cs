using System;
using System.IO;
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
}
public class MyPriorityQueue<T> where T : IComparable<T>
{
    private Heap<T> heap;
    public MyPriorityQueue()
    {
        heap = new Heap<T>(true);
    }
    public int Count => heap.Count;
    public bool IsEmpty => heap.IsEmpty;
    public void Add(T element)
    {
        heap.Add(element);
    }
    public T Peek()
    {
        if (IsEmpty) throw new InvalidOperationException("Queue is empty");
        return heap.Peek();
    }
    public T Poll()
    {
        if (IsEmpty) throw new InvalidOperationException("Queue is empty");
        return heap.RemoveRoot();
    }
}
public class Request : IComparable<Request>
{
    public int Priority { get; set; }
    public int RequestId { get; set; }
    public int StepAdded { get; set; }
    public int StepProcessed { get; set; }
    public Request(int priority, int requestId, int stepAdded)
    {
        Priority = priority;
        RequestId = requestId;
        StepAdded = stepAdded;
        StepProcessed = -1;
    }
    public int CompareTo(Request other)
    {
        return this.Priority.CompareTo(other.Priority);
    }
    public int WaitingTime
    {
        get
        {
            if (StepProcessed == -1) return 0;
            return StepProcessed - StepAdded;
        }
    }
}
class Program
{
    static void Main()
    {
        Console.WriteLine(" СИСТЕМА ОБРАБОТКИ ЗАЯВОК");
        File.WriteAllText("log.txt", "");
        MyPriorityQueue<Request> queue = new MyPriorityQueue<Request>();
        int currentRequestId = 1;
        int currentStep = 0;
        Request longestWaitingRequest = null;
        Random rand = new Random();
        Console.Write("Введите количество шагов добавления заявок N: ");
        int totalSteps = int.Parse(Console.ReadLine());
        Console.WriteLine($"\n 1: {totalSteps} ШАГОВ ДОБАВЛЕНИЯ ЗАЯВОК ");
        for (int step = 1; step <= totalSteps; step++)
        {
            currentStep = step;
            int numRequests = rand.Next(1, 11);
            for (int i = 0; i < numRequests; i++)
            {
                int priority = rand.Next(1, 6); 
                Request request = new Request(priority, currentRequestId, currentStep);
                queue.Add(request);
                LogAction("ADD", request, currentStep);
                currentRequestId++;
            }
            if (!queue.IsEmpty)
            {
                Request processed = queue.Poll();
                processed.StepProcessed = currentStep;
                if (longestWaitingRequest == null || processed.WaitingTime > longestWaitingRequest.WaitingTime)
                {
                    longestWaitingRequest = processed;
                }
                LogAction("REMOVE", processed, currentStep);
            }
        }
        Console.WriteLine($"\n 2: ОБРАБОТКА ОСТАВШИХСЯ ЗАЯВОК");
        while (!queue.IsEmpty)
        {
            currentStep++;
            Request processed = queue.Poll();
            processed.StepProcessed = currentStep;
            if (longestWaitingRequest == null || processed.WaitingTime > longestWaitingRequest.WaitingTime)
            {
                longestWaitingRequest = processed;
            }
            LogAction("REMOVE", processed, currentStep);
        }
        Console.WriteLine("\nРЕЗУЛЬТАТЫ");
        if (longestWaitingRequest != null)
        {
            Console.WriteLine("Заявка с максимальным временем ожидания:");
            Console.WriteLine($"  Номер заявки: {longestWaitingRequest.RequestId}");
            Console.WriteLine($"  Приоритет: {longestWaitingRequest.Priority}");
            Console.WriteLine($"  Шаг добавления: {longestWaitingRequest.StepAdded}");
            Console.WriteLine($"  Шаг обработки: {longestWaitingRequest.StepProcessed}");
            Console.WriteLine($"  Время ожидания: {longestWaitingRequest.WaitingTime} шагов");
        }
        Console.WriteLine($"\nЛог операций сохранен в файл: log.txt");
    }
    static void LogAction(string action, Request request, int step)
    {
        string logEntry = $"{action} {request.RequestId} {request.Priority} {step}";
        File.AppendAllText("log.txt", logEntry + Environment.NewLine);
    }
}
