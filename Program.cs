using System;
public class TaskRequest
{
    public int Id;
    public int Priority;
    public int AddedStep;
    public TaskRequest(int id, int priority, int step)
    {
        Id = id;
        Priority = priority;
        AddedStep = step;
    }
}
class Program
{
    static void Main()
    {
        Console.Write("Enter number of steps: ");
        int totalSteps = int.Parse(Console.ReadLine());
        TaskRequest[] requests = new TaskRequest[1000];
        int requestCount = 0;
        Random random = new Random();
        int nextId = 1;
        Console.WriteLine("\n=== Processing Steps ===");
        for (int step = 1; step <= totalSteps; step++)
        {
            int addCount = random.Next(1, 4);
            Console.WriteLine($"\nStep {step}: Adding {addCount} requests");

            for (int i = 0; i < addCount; i++)
            {
                int priority = random.Next(1, 6);
                requests[requestCount] = new TaskRequest(nextId, priority, step);
                requestCount++;
                Console.WriteLine($"  Added request {nextId} (priority {priority})");
                nextId++;
            }
            if (requestCount > 0)
            {
                int highestPriorityIndex = 0;
                for (int i = 1; i < requestCount; i++)
                {
                    if (requests[i].Priority > requests[highestPriorityIndex].Priority)
                    {
                        highestPriorityIndex = i;
                    }
                }
                TaskRequest processed = requests[highestPriorityIndex];
                Console.WriteLine($"  Processed request {processed.Id} (priority {processed.Priority})");
                for (int i = highestPriorityIndex; i < requestCount - 1; i++)
                {
                    requests[i] = requests[i + 1];
                }
                requestCount--;
            }
        }
        Console.WriteLine("\n=== Processing Remaining Requests ===");
        int remainingStep = totalSteps + 1;
        while (requestCount > 0)
        {
            int highestPriorityIndex = 0;
            for (int i = 1; i < requestCount; i++)
            {
                if (requests[i].Priority > requests[highestPriorityIndex].Priority)
                {
                    highestPriorityIndex = i;
                }
            }
            TaskRequest processed = requests[highestPriorityIndex];
            Console.WriteLine($"Step {remainingStep}: Processed request {processed.Id} (priority {processed.Priority})");
            for (int i = highestPriorityIndex; i < requestCount - 1; i++)
            {
                requests[i] = requests[i + 1];
            }
            requestCount--;
            remainingStep++;
        }
        Console.WriteLine("\nAll requests processed!");
    }
}