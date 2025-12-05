using System;
using System.Collections.Generic;
public class MyLinkedList<T>
{
    private class Node
    {
        public T Data;
        public Node Next;
        public Node Previous;
        public Node(T data)
        {
            Data = data;
        }
    }
    private Node first;
    private Node last;
    private int size;
    public MyLinkedList()
    {
        first = null;
        last = null;
        size = 0;
    }
    public MyLinkedList(T[] array)
    {
        if (array == null) return;
        foreach (T item in array) Add(item);
    }
    public void Add(T item)
    {
        Node newNode = new Node(item);
        if (first == null)
        {
            first = newNode;
            last = newNode;
        }
        else
        {
            last.Next = newNode;
            newNode.Previous = last;
            last = newNode;
        }
        size++;
    }
    public void AddFirst(T item)
    {
        Node newNode = new Node(item);
        if (first == null)
        {
            first = newNode;
            last = newNode;
        }
        else
        {
            newNode.Next = first;
            first.Previous = newNode;
            first = newNode;
        }
        size++;
    }
    public void AddLast(T item)
    {
        Add(item);
    }
    public bool Remove(T item)
    {
        Node current = first;
        while (current != null)
        {
            if (current.Data.Equals(item))
            {
                if (current.Previous != null)
                    current.Previous.Next = current.Next;
                else
                    first = current.Next;
                if (current.Next != null)
                    current.Next.Previous = current.Previous;
                else
                    last = current.Previous;
                size--;
                return true;
            }
            current = current.Next;
        }
        return false;
    }
    public T RemoveFirst()
    {
        if (first == null) throw new InvalidOperationException("Список пуст");
        T data = first.Data;
        first = first.Next;
        if (first != null)
            first.Previous = null;
        else
            last = null;
        size--;
        return data;
    }
    public T RemoveLast()
    {
        if (last == null) throw new InvalidOperationException("Список пуст");
        T data = last.Data;
        last = last.Previous;
        if (last != null)
            last.Next = null;
        else
            first = null;
        size--;
        return data;
    }
    public void Clear()
    {
        first = null;
        last = null;
        size = 0;
    }
    public bool Contains(T item)
    {
        Node current = first;
        while (current != null)
        {
            if (current.Data.Equals(item))
                return true;
            current = current.Next;
        }
        return false;
    }
    public T GetFirst()
    {
        if (first == null) throw new InvalidOperationException("Список пуст");
        return first.Data;
    }
    public T GetLast()
    {
        if (last == null) throw new InvalidOperationException("Список пуст");
        return last.Data;
    }
    public bool IsEmpty()
    {
        return size == 0;
    }
    public int Size()
    {
        return size;
    }
    public T[] ToArray()
    {
        T[] array = new T[size];
        Node current = first;
        int i = 0;
        while (current != null)
        {
            array[i] = current.Data;
            current = current.Next;
            i++;
        }
        return array;
    }
    public void Print()
    {
        Console.Write("[");
        Node current = first;
        while (current != null)
        {
            Console.Write(current.Data);
            if (current.Next != null) Console.Write(", ");
            current = current.Next;
        }
        Console.WriteLine($"] (размер: {size})");
    }
}
class Program
{
    static void Main()
    {
        MyLinkedList<int> list = new MyLinkedList<int>();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Двунаправленный список ===");
            Console.Write("Список: ");
            list.Print();
            Console.WriteLine();
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Добавить в начало");
            Console.WriteLine("3. Добавить в конец");
            Console.WriteLine("4. Удалить элемент");
            Console.WriteLine("5. Удалить первый");
            Console.WriteLine("6. Удалить последний");
            Console.WriteLine("7. Проверить наличие");
            Console.WriteLine("8. Получить первый");
            Console.WriteLine("9. Получить последний");
            Console.WriteLine("10. Размер");
            Console.WriteLine("11. Очистить");
            Console.WriteLine("12. В массив");
            Console.WriteLine("13. Создать из массива");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите: ");
            string choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите число: ");
                        int num = int.Parse(Console.ReadLine());
                        list.Add(num);
                        Console.WriteLine("Добавлено");
                        break;
                    case "2":
                        Console.Write("Введите число: ");
                        num = int.Parse(Console.ReadLine());
                        list.AddFirst(num);
                        Console.WriteLine("Добавлено в начало");
                        break;

                    case "3":
                        Console.Write("Введите число: ");
                        num = int.Parse(Console.ReadLine());
                        list.AddLast(num);
                        Console.WriteLine("Добавлено в конец");
                        break;
                    case "4":
                        Console.Write("Введите число для удаления: ");
                        num = int.Parse(Console.ReadLine());
                        bool removed = list.Remove(num);
                        Console.WriteLine(removed ? "Удалено" : "Не найдено");
                        break;
                    case "5":
                        try
                        {
                            int first = list.RemoveFirst();
                            Console.WriteLine($"Удален первый: {first}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "6":
                        try
                        {
                            int last = list.RemoveLast();
                            Console.WriteLine($"Удален последний: {last}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "7":
                        Console.Write("Введите число для проверки: ");
                        num = int.Parse(Console.ReadLine());
                        bool contains = list.Contains(num);
                        Console.WriteLine(contains ? "Найдено" : "Не найдено");
                        break;
                    case "8":
                        try
                        {
                            int firstVal = list.GetFirst();
                            Console.WriteLine($"Первый элемент: {firstVal}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "9":
                        try
                        {
                            int lastVal = list.GetLast();
                            Console.WriteLine($"Последний элемент: {lastVal}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        break;
                    case "10":
                        Console.WriteLine($"Размер списка: {list.Size()}");
                        break;
                    case "11":
                        list.Clear();
                        Console.WriteLine("Список очищен");
                        break;
                    case "12":
                        int[] array = list.ToArray();
                        Console.WriteLine("Массив: [" + string.Join(", ", array) + "]");
                        break;
                    case "13":
                        Console.Write("Введите числа через пробел: ");
                        string input = Console.ReadLine();
                        string[] parts = input.Split(' ');
                        int[] nums = new int[parts.Length];
                        for (int i = 0; i < parts.Length; i++)
                        {
                            nums[i] = int.Parse(parts[i]);
                        }
                        list = new MyLinkedList<int>(nums);
                        Console.WriteLine("Создан новый список");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введите число");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
            Console.WriteLine("\nEnter");
            Console.ReadLine();
        }
    }
}