using System;
public class MyArrayDeque<T>
{
    private T[] elements;
    private int head;
    private int tail;
    private int size;
    public MyArrayDeque()
    {
        elements = new T[16];
        head = 0;
        tail = 0;
        size = 0;
    }
    public MyArrayDeque(T[] array)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        elements = new T[Math.Max(array.Length * 2, 16)];
        Array.Copy(array, elements, array.Length);
        head = 0;
        tail = array.Length;
        size = array.Length;
    }
    public MyArrayDeque(int capacity)
    {
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        elements = new T[capacity];
        head = 0;
        tail = 0;
        size = 0;
    }
    public int Size() => size;
    public bool IsEmpty() => size == 0;
    private void EnsureCapacity()
    {
        if (size == elements.Length)
        {
            T[] newElements = new T[elements.Length * 2];
            if (head < tail)
            {
                Array.Copy(elements, head, newElements, 0, size);
            }
            else
            {
                Array.Copy(elements, head, newElements, 0, elements.Length - head);
                Array.Copy(elements, 0, newElements, elements.Length - head, tail);
            }
            head = 0;
            tail = size;
            elements = newElements;
        }
    }
    public void AddFirst(T item)
    {
        EnsureCapacity();
        head = (head - 1 + elements.Length) % elements.Length;
        elements[head] = item;
        size++;
    }
    public void AddLast(T item)
    {
        EnsureCapacity();
        elements[tail] = item;
        tail = (tail + 1) % elements.Length;
        size++;
    }
    public T RemoveFirst()
    {
        if (size == 0) throw new InvalidOperationException("Дек пуст");
        T result = elements[head];
        elements[head] = default(T);
        head = (head + 1) % elements.Length;
        size--;
        return result;
    }
    public T RemoveLast()
    {
        if (size == 0) throw new InvalidOperationException("Дек пуст");
        tail = (tail - 1 + elements.Length) % elements.Length;
        T result = elements[tail];
        elements[tail] = default(T);
        size--;
        return result;
    }
    public T PeekFirst()
    {
        if (size == 0) throw new InvalidOperationException("Дек пуст");
        return elements[head];
    }
    public T PeekLast()
    {
        if (size == 0) throw new InvalidOperationException("Дек пуст");
        return elements[(tail - 1 + elements.Length) % elements.Length];
    }
    public void Clear()
    {
        Array.Clear(elements, 0, elements.Length);
        head = 0;
        tail = 0;
        size = 0;
    }
    public bool Contains(T item)
    {
        var comparer = System.Collections.Generic.EqualityComparer<T>.Default;
        for (int i = 0; i < size; i++)
        {
            int index = (head + i) % elements.Length;
            if (comparer.Equals(elements[index], item))
                return true;
        }
        return false;
    }
    public T[] ToArray()
    {
        T[] result = new T[size];
        if (size == 0) return result;
        if (head < tail)
        {
            Array.Copy(elements, head, result, 0, size);
        }
        else
        {
            int firstPart = elements.Length - head;
            Array.Copy(elements, head, result, 0, firstPart);
            Array.Copy(elements, 0, result, firstPart, tail);
        }
        return result;
    }
    public void PrintAll()
    {
        Console.Write("Дек содержит: ");
        if (size == 0)
        {
            Console.WriteLine("пусто");
            return;
        }
        for (int i = 0; i < size; i++)
        {
            int index = (head + i) % elements.Length;
            Console.Write(elements[index] + " ");
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main()
    {
        MyArrayDeque<int> deque = new MyArrayDeque<int>();
        bool running = true;
        Console.WriteLine(" двунаправленная очередь (дек)");
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("1  - Добавить элемент в начало");
        Console.WriteLine("2  - Добавить элемент в конец");
        Console.WriteLine("3  - Удалить элемент из начала");
        Console.WriteLine("4  - Удалить элемент из конца");
        Console.WriteLine("5  - Показать первый элемент");
        Console.WriteLine("6  - Показать последний элемент");
        Console.WriteLine("7  - Проверить наличие элемента");
        Console.WriteLine("8  - Показать все элементы");
        Console.WriteLine("9  - Очистить дек");
        Console.WriteLine("10 - Показать размер");
        Console.WriteLine("0  - Выход");
        while (running)
        {
            try
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.Write("Введите число для добавления в начало: ");
                        if (int.TryParse(Console.ReadLine(), out int val1))
                        {
                            deque.AddFirst(val1);
                            Console.WriteLine($"Добавлено {val1} в начало");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода числа");
                        }
                        break;

                    case "2":
                        Console.Write("Введите число для добавления в конец: ");
                        if (int.TryParse(Console.ReadLine(), out int val2))
                        {
                            deque.AddLast(val2);
                            Console.WriteLine($"Добавлено {val2} в конец");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода числа");
                        }
                        break;

                    case "3":
                        try
                        {
                            int removedFirst = deque.RemoveFirst();
                            Console.WriteLine($"Удалён первый элемент: {removedFirst}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "4":
                        try
                        {
                            int removedLast = deque.RemoveLast();
                            Console.WriteLine($"Удалён последний элемент: {removedLast}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "5":
                        try
                        {
                            int first = deque.PeekFirst();
                            Console.WriteLine($"Первый элемент: {first}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "6":
                        try
                        {
                            int last = deque.PeekLast();
                            Console.WriteLine($"Последний элемент: {last}");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                        }
                        break;

                    case "7":
                        Console.Write("Введите число для поиска: ");
                        if (int.TryParse(Console.ReadLine(), out int searchVal))
                        {
                            bool contains = deque.Contains(searchVal);
                            Console.WriteLine($"Элемент {searchVal} {(contains ? "найден" : "не найден")}");
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода числа");
                        }
                        break;

                    case "8":
                        deque.PrintAll();
                        break;

                    case "9":
                        deque.Clear();
                        Console.WriteLine("Дек очищен");
                        break;
                    case "10":
                        Console.WriteLine($"Размер дека: {deque.Size()}");
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Выход из программы");
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда. Попробуйте снова.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
        //Пример работы с массивом
        Console.WriteLine("\n--- Пример создания дека из массива ---");
        int[] initialArray = { 100, 200, 300 };
        MyArrayDeque<int> dequeFromArray = new MyArrayDeque<int>(initialArray);
        Console.WriteLine("Исходный массив: 100, 200, 300");
        Console.Write("Дек из массива: ");
        dequeFromArray.PrintAll();
        Console.WriteLine("Добавляем 50 в начало и 400 в конец...");
        dequeFromArray.AddFirst(50);
        dequeFromArray.AddLast(400);
        Console.Write("Результат: ");
        dequeFromArray.PrintAll();
        Console.WriteLine($"Первый элемент: {dequeFromArray.PeekFirst()}");
        Console.WriteLine($"Последний элемент: {dequeFromArray.PeekLast()}");
        // Преобразование в массив
        int[] arrayResult = dequeFromArray.ToArray();
        Console.Write("Дек как массив: ");
        foreach (var item in arrayResult)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}