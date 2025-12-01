using System;
using System.IO;
using System.Linq;
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
        if (size == 0)
        {
            Console.WriteLine("Дек пуст");
            return;
        }
        for (int i = 0; i < size; i++)
        {
            int index = (head + i) % elements.Length;
            Console.WriteLine($"  [{i}]: {elements[index]}");
        }
    }
    //для задачи 15
    public T GetFirst()
    {
        if (size == 0) throw new InvalidOperationException("Дек пуст");
        return elements[head];
    }
    public void Add(T item)
    {
        AddLast(item);
    }
    public T[] ToArray(T[] array)
    {
        T[] result = ToArray();
        if (array != null && array.Length >= size)
        {
            Array.Copy(result, array, size);
            return array;
        }
        return result;
    }
}
class Program
{
    //Функция для подсчёта цифр в строке
    static int CountDigits(string str)
    {
        int count = 0;
        foreach (char c in str)
        {
            if (char.IsDigit(c))
                count++;
        }
        return count;
    }
    //Функция для подсчёта пробелов в строке
    static int CountSpaces(string str)
    {
        int count = 0;
        foreach (char c in str)
        {
            if (c == ' ')
                count++;
        }
        return count;
    }
    static void Main()
    {
        try
        {
            Console.WriteLine("Обработка строк в двунаправленной очереди");
            //Чтение строк из файла input.txt
            Console.WriteLine("Чтение строк из файла input.txt");
            string[] lines;
            if (File.Exists("input.txt"))
            {
                lines = File.ReadAllLines("input.txt");
                Console.WriteLine($"Прочитано {lines.Length} строк из файла.");
            }
            else
            {
                Console.WriteLine("Файл input.txt не найден");
                string[] testLines = {
                    "Строка 1 содержит 3 цифры: 123",
                    "А это строка с 2024 годом и еще 5 цифрами",
                    "Простая строка без цифр",
                    "Еще 1 строка с 5 цифрами 67890",
                    "Последняя строка 99"
                };
                File.WriteAllLines("input.txt", testLines);
                lines = testLines;
                Console.WriteLine("Создан тестовый файл input.txt");
            }
            //Создание и заполнение дека по правилу задачи
            Console.WriteLine("\nЗаполнение дека по правилу:");
            Console.WriteLine("Если строка содержит больше цифр, чем первая строка - в конец, иначе в начало");
            MyArrayDeque<string> deque = new MyArrayDeque<string>();
            if (lines.Length > 0)
            {
                deque.Add(lines[0]);
                int firstLineDigits = CountDigits(lines[0]);
                Console.WriteLine($"Первая строка: '{lines[0]}' (цифр: {firstLineDigits})");
                Console.WriteLine($"Добавлена в дек");
                for (int i = 1; i < lines.Length; i++)
                {
                    int currentDigits = CountDigits(lines[i]);
                    Console.WriteLine($"\nСтрока {i + 1}: '{lines[i]}' (цифр: {currentDigits})");
                    if (currentDigits > firstLineDigits)
                    {
                        deque.AddLast(lines[i]);
                        Console.WriteLine($"Добавлена в КОНЕЦ дека (цифр больше)");
                    }
                    else
                    {
                        deque.AddFirst(lines[i]);
                        Console.WriteLine($"Добавлена в НАЧАЛО дека (цифр не больше)");
                    }
                }
            }
            //Вывод содержимого дека
            Console.WriteLine("\nСодержимое дека после заполнения:");
            deque.PrintAll();
            //Сохранение результата в файл sorted.txt
            Console.WriteLine("\nСохранение результата в файл sorted.txt");
            string[] dequeArray = deque.ToArray();
            File.WriteAllLines("sorted.txt", dequeArray);
            Console.WriteLine($"Сохранено {dequeArray.Length} строк в sorted.txt");
            //Удаление строк с более чем n пробелами
            Console.WriteLine("\nУдаление строк с большим количеством пробелов");
            Console.Write("Введите максимальное допустимое количество пробелов (n): ");
            if (int.TryParse(Console.ReadLine(), out int maxSpaces) && maxSpaces >= 0)
            {
                Console.WriteLine($"\nУдаление строк, содержащих более {maxSpaces} пробелов");
                string[] tempArray = deque.ToArray();
                deque.Clear(); 
                int removedCount = 0;
                foreach (string line in tempArray)
                {
                    int spaceCount = CountSpaces(line);
                    if (spaceCount <= maxSpaces)
                    {
                        deque.Add(line);
                    }
                    else
                    {
                        removedCount++;
                        Console.WriteLine($"Удалена строка: '{line}' (пробелов: {spaceCount})");
                    }
                }
                Console.WriteLine($"\nУдалено {removedCount} строк.");
                //Вывод оставшихся строк на экран
                Console.WriteLine("\nОставшиеся строки в деке:");
                if (deque.Size() == 0)
                {
                    Console.WriteLine("Все строки удалены. Дек пуст.");
                }
                else
                {
                    Console.WriteLine($"Осталось {deque.Size()} строк:");
                    deque.PrintAll();
                }
                string[] remainingLines = deque.ToArray();
                File.WriteAllLines("filtered.txt", remainingLines);
                Console.WriteLine($"Оставшиеся строки также сохранены в файл filtered.txt");
            }
            else
            {
                Console.WriteLine("Ошибка: введено недопустимое значение. Должно быть целое число >= 0.");
            }
            //Статистика по файлам
            Console.WriteLine("\n Статистика по файлам");
            if (File.Exists("sorted.txt"))
            {
                string[] sortedLines = File.ReadAllLines("sorted.txt");
                Console.WriteLine($"Файл sorted.txt: {sortedLines.Length} строк");
            }
            if (File.Exists("filtered.txt"))
            {
                string[] filteredLines = File.ReadAllLines("filtered.txt");
                Console.WriteLine($"Файл filtered.txt: {filteredLines.Length} строк");
            }
            //Пример содержимого файлов
            Console.WriteLine("\nСодержимое файла sorted.txt:");
            if (File.Exists("sorted.txt"))
            {
                int lineNum = 1;
                foreach (string line in File.ReadLines("sorted.txt"))
                {
                    Console.WriteLine($"{lineNum:00}. {line} (цифр: {CountDigits(line)}, пробелов: {CountSpaces(line)})");
                    lineNum++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            Console.WriteLine("Подробности: " + ex.StackTrace);
        }
        Console.WriteLine("\nНажмите любую кнопку для выхода");
        Console.ReadKey();
    }
}