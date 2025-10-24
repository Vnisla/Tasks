using System;
public class PriorityQueue
{
    private int[] items;
    private int count;
    public PriorityQueue()
    {
        items = new int[10];
        count = 0;
    }
    public void Add(int value)
    {
        if (count == items.Length)
        {
            ResizeArray();
        }
        int position = 0;
        while (position < count && items[position] <= value)
        {
            position++;
        }
        for (int i = count; i > position; i--)
        {
            items[i] = items[i - 1];
        }
        items[position] = value;
        count++;
    }
    public int Remove()
    {
        if (count == 0)
            throw new Exception("Queue is empty");
        int value = items[0];
        for (int i = 0; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }
        count--;
        return value;
    }
    public int Peek()
    {
        if (count == 0)
            throw new Exception("Queue is empty");
        return items[0];
    }
    public bool IsEmpty()
    {
        return count == 0;
    }
    public int Size()
    {
        return count;
    }
    private void ResizeArray()
    {
        int[] newArray = new int[items.Length * 2];
        for (int i = 0; i < count; i++)
        {
            newArray[i] = items[i];
        }
        items = newArray;
    }
    public void Print()
    {
        Console.Write("Priority queue: ");
        for (int i = 0; i < count; i++)
        {
            Console.Write(items[i] + " ");
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        PriorityQueue pq = new PriorityQueue();
        pq.Add(30);
        pq.Add(10);
        pq.Add(40);
        pq.Add(20);
        pq.Add(5);
        pq.Print();
        Console.WriteLine("Front element: " + pq.Peek());
        Console.WriteLine("Queue size: " + pq.Size());
        Console.WriteLine("Processing queue:");
        while (!pq.IsEmpty())
        {
            Console.WriteLine("Processed: " + pq.Remove());
            pq.Print();
        }
    }
}