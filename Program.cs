using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
public class MyVector<T> : IEnumerable<T>
{
    private T[] elementData;
    private int elementCount;
    private int capacityIncrement;
    public MyVector(int initialCapacity, int capacityIncrement)
    {
        if (initialCapacity < 0)
            throw new ArgumentException("Начальная емкость не может быть отрицательной");
        this.elementData = new T[initialCapacity];
        this.elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }
    public MyVector(int initialCapacity) : this(initialCapacity, 0) { }
    public MyVector() : this(10, 0) { }
    public MyVector(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        this.elementData = new T[a.Length];
        Array.Copy(a, 0, elementData, 0, a.Length);
        this.elementCount = a.Length;
        this.capacityIncrement = 0;
    }
    public int Count => elementCount;
    public int Capacity => elementData.Length;
    public void Add(T e)
    {
        EnsureCapacity(elementCount + 1);
        elementData[elementCount++] = e;
    }
    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        EnsureCapacity(elementCount + a.Length);
        Array.Copy(a, 0, elementData, elementCount, a.Length);
        elementCount += a.Length;
    }
    public void Clear()
    {
        Array.Clear(elementData, 0, elementCount);
        elementCount = 0;
    }
    public bool Contains(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < elementCount; i++)
            {
                if (elementData[i] == null)
                    return true;
            }
        }
        else
        {
            for (int i = 0; i < elementCount; i++)
            {
                if (o.Equals(elementData[i]))
                    return true;
            }
        }
        return false;
    }
    public bool ContainsAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        foreach (var item in a)
        {
            if (!Contains(item))
                return false;
        }
        return true;
    }
    public bool IsEmpty() => elementCount == 0;
    public bool Remove(object o)
    {
        int index = IndexOf(o);
        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }
        return false;
    }
    public void RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        foreach (var item in a)
        {
            Remove(item);
        }
    }
    public void RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        for (int i = elementCount - 1; i >= 0; i--)
        {
            if (Array.IndexOf(a, elementData[i]) == -1)
            {
                RemoveAt(i);
            }
        }
    }
    public T[] ToArray()
    {
        T[] result = new T[elementCount];
        Array.Copy(elementData, 0, result, 0, elementCount);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null)
            return ToArray();
        if (a.Length < elementCount)
        {
            return ToArray();
        }
        Array.Copy(elementData, 0, a, 0, elementCount);
        if (a.Length > elementCount)
        {
            a[elementCount] = default(T);
        }
        return a;
    }
    public void Add(int index, T e)
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity(elementCount + 1);
        if (index < elementCount)
        {
            Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        }
        elementData[index] = e;
        elementCount++;
    }
    public void AddAll(int index, T[] a)
    {
        if (index < 0 || index > elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        int numNew = a.Length;
        EnsureCapacity(elementCount + numNew);
        if (index < elementCount)
        {
            Array.Copy(elementData, index, elementData, index + numNew, elementCount - index);
        }
        Array.Copy(a, 0, elementData, index, numNew);
        elementCount += numNew;
    }
    public T Get(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }
    public int IndexOf(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < elementCount; i++)
            {
                if (elementData[i] == null)
                    return i;
            }
        }
        else
        {
            for (int i = 0; i < elementCount; i++)
            {
                if (o.Equals(elementData[i]))
                    return i;
            }
        }
        return -1;
    }
    public int LastIndexOf(object o)
    {
        if (o == null)
        {
            for (int i = elementCount - 1; i >= 0; i--)
            {
                if (elementData[i] == null)
                    return i;
            }
        }
        else
        {
            for (int i = elementCount - 1; i >= 0; i--)
            {
                if (o.Equals(elementData[i]))
                    return i;
            }
        }
        return -1;
    }
    public T RemoveAt(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        int numMoved = elementCount - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[--elementCount] = default(T);
        return oldValue;
    }
    public T Set(int index, T e)
    {
        if (index < 0 || index >= elementCount)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }
    public MyVector<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int subListSize = toIndex - fromIndex;
        T[] subArray = new T[subListSize];
        Array.Copy(elementData, fromIndex, subArray, 0, subListSize);
        return new MyVector<T>(subArray);
    }
    public T FirstElement()
    {
        if (elementCount == 0)
            throw new InvalidOperationException("Вектор пуст");
        return elementData[0];
    }
    public T LastElement()
    {
        if (elementCount == 0)
            throw new InvalidOperationException("Вектор пуст");
        return elementData[elementCount - 1];
    }
    public void RemoveElementAt(int pos)
    {
        RemoveAt(pos);
    }
    public void RemoveRange(int begin, int end)
    {
        if (begin < 0 || end > elementCount || begin > end)
            throw new ArgumentOutOfRangeException();
        int numMoved = elementCount - end;
        Array.Copy(elementData, end, elementData, begin, numMoved);
        int newSize = elementCount - (end - begin);
        Array.Clear(elementData, newSize, end - begin);
        elementCount = newSize;
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity;
            if (capacityIncrement > 0)
            {
                newCapacity = elementData.Length + capacityIncrement;
            }
            else
            {
                newCapacity = elementData.Length * 2;
            }
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            T[] newElementData = new T[newCapacity];
            Array.Copy(elementData, 0, newElementData, 0, elementCount);
            elementData = newElementData;
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < elementCount; i++)
        {
            yield return elementData[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
public class IPAddressProcessor
{
    public static void ProcessIPAddresses(string inputFile, string outputFile)
    {
        try
        {
            Console.WriteLine($"Начало обработки файла: {inputFile}");
            MyVector<string> linesVector = ReadLinesFromFile(inputFile);
            Console.WriteLine($"Прочитано строк из файла: {linesVector.Count}");
            MyVector<string> ipAddresses = ExtractValidIPAddresses(linesVector);
            Console.WriteLine($"Найдено IP-адресов: {ipAddresses.Count}");
            WriteIPAddressesToFile(ipAddresses, outputFile);

            Console.WriteLine($"Обработка завершена успешно!");
            Console.WriteLine($"Результат записан в файл: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
        }
    }
    private static MyVector<string> ReadLinesFromFile(string filePath)
    {
        MyVector<string> lines = new MyVector<string>();
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл {filePath} не найден");
        }
        using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
        {
            string line;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line.Trim());
                }
            }
            Console.WriteLine($"Обработано строк: {lineNumber}");
        }
        return lines;
    }
    private static MyVector<string> ExtractValidIPAddresses(MyVector<string> lines)
    {
        MyVector<string> ipAddresses = new MyVector<string>();
        int totalFound = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines.Get(i);
            int foundInLine = ExtractIPsFromLine(line, ipAddresses);
            totalFound += foundInLine;
            if (foundInLine > 0)
            {
                Console.WriteLine($"В строке {i + 1} найдено IP-адресов: {foundInLine}");
            }
        }
        Console.WriteLine($"Всего обнаружено потенциальных IP-адресов: {totalFound}");
        return ipAddresses;
    }
    private static int ExtractIPsFromLine(string line, MyVector<string> ipAddresses)
    {
        int foundCount = 0;
        for (int i = 0; i < line.Length; i++)
        {
            if (char.IsDigit(line[i]))
            {
                int start = i;
                int end = FindIPEnd(line, i);
                if (end > start)
                {
                    string potentialIP = line.Substring(start, end - start);
                    foundCount++;
                    if (IsValidIPv4(potentialIP) && !OverlapsWithOtherNumbers(line, start, end))
                    {
                        Console.WriteLine($"Найден валидный IP: {potentialIP}");
                        ipAddresses.Add(potentialIP);
                    }
                    else
                    {
                        Console.WriteLine($"Невалидный IP (пропущен): {potentialIP}");
                    }
                    i = end - 1;
                }
            }
        }
        return foundCount;
    }
    private static int FindIPEnd(string line, int start)
    {
        int dotCount = 0;
        int i = start;
        int lastDigitPosition = start;
        while (i < line.Length && dotCount < 4)
        {
            char c = line[i];
            if (c == '.')
            {
                dotCount++;
                if (dotCount == 3)
                {
                    int j = i + 1;
                    while (j < line.Length && char.IsDigit(line[j]))
                    {
                        j++;
                    }
                    return j;
                }
                if (i + 1 >= line.Length || !char.IsDigit(line[i + 1]))
                {
                    return -1;
                }
            }
            else if (!char.IsDigit(c))
            {
                break;
            }
            else
            {
                lastDigitPosition = i;
            }
            i++;
        }
        return -1; 
    }
    private static bool IsValidIPv4(string ip)
    {
        if (ip.Length < 7 || ip.Length > 15) 
            return false;
        string[] parts = ip.Split('.');
        if (parts.Length != 4)
            return false;
        foreach (string part in parts)
        {
            if (string.IsNullOrEmpty(part))
                return false;
            if (part.Length > 1 && part[0] == '0')
                return false;
            foreach (char c in part)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            if (!int.TryParse(part, out int octet) || octet < 0 || octet > 255)
                return false;
        }
        return true;
    }
    private static bool OverlapsWithOtherNumbers(string line, int start, int end)
    {
        if (start > 0 && char.IsDigit(line[start - 1]))
        {
            Console.WriteLine($"IP {line.Substring(start, end - start)} пересекается с цифрами перед ним");
            return true;
        }
        if (end < line.Length && char.IsDigit(line[end]))
        {
            Console.WriteLine($"IP {line.Substring(start, end - start)} пересекается с цифрами после него");
            return true;
        }
        return false;
    }
    private static void WriteIPAddressesToFile(MyVector<string> ipAddresses, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            for (int i = 0; i < ipAddresses.Count; i++)
            {
                writer.WriteLine(ipAddresses.Get(i));
            }
        }
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.WriteLine("Извлечение IP-адресов из файла");
        Console.WriteLine("класс MyVector");
        string inputFile = "input.txt";
        string outputFile = "output.txt";
        if (!File.Exists(inputFile))
        {
            CreateTestInputFile(inputFile);
        }
        ShowFileContent(inputFile, "Входной файл");
        Console.WriteLine("\n" + new string(' ', 50));
        Console.WriteLine("НАЧАЛО ОБРАБОТКИ");
        Console.WriteLine(new string('=', 50));
        IPAddressProcessor.ProcessIPAddresses(inputFile, outputFile);
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("РЕЗУЛЬТАТЫ ОБРАБОТКИ");
        Console.WriteLine(new string('=', 50));
        ShowResults(outputFile);
        ShowAnalysis();
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("Обработка завершена. Нажмите любую кнопулю");
        Console.ReadKey();
    }
    private static void CreateTestInputFile(string filePath)
    {
        Console.WriteLine($"Создание тестового файла {filePath}...");
        string[] testLines = {
            "Сервер 192.168.1.1 доступен, а 192.168.1.256 нет",
            "IP адреса: 10.0.0.1, 172.16.254.1, 8.8.8.8",
            "Некорректные адреса: 121.121.121.121.2, 111.111.111.1111, 300.400.500.600",
            "Локальные адреса: 127.0.0.1, 192.168.0.1, 10.0.0.254",
            "Граничные случаи: 0.0.0.0, 255.255.255.255",
            "Пересечения с числами: 121.121.121.121.2 и 111.111.111.1111 не должны учитываться",
            "Тест с ведущими нулями: 010.010.010.010 - невалидный",
            "Разные форматы: 1.2.3.4, 11.22.33.44, 101.102.103.104",
            "Пустая строка и пробелы:",
            "   ",
            "Строка без IP-адресов: просто текст с числами 12345",
            "Корректные в середине текста: подключитесь к 203.0.113.195 для доступа",
            "Множественные адреса: 198.51.100.1 и 203.0.113.254 работают",
            "Невалидные символы: 192.168.1.1a, a192.168.1.1, 192.168.1.",
            "Специальные случаи: .192.168.1.1, 192.168.1.1."
        };
        try
        {
            File.WriteAllLines(filePath, testLines, Encoding.UTF8);
            Console.WriteLine($"Файл {filePath} успешно создан");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании файла: {ex.Message}");
        }
    }
    private static void ShowFileContent(string filePath, string title)
    {
        Console.WriteLine($"\n{title}: {filePath}");
        Console.WriteLine(new string('-', 60));
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не существует");
            return;
        }
        try
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine($"{i + 1,3}: {lines[i]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
        }
        Console.WriteLine(new string('-', 60));
    }
    private static void ShowResults(string outputFile)
    {
        ShowFileContent(outputFile, "Выходной файл с IP-адресами");
        if (File.Exists(outputFile))
        {
            string[] ipAddresses = File.ReadAllLines(outputFile, Encoding.UTF8);
            Console.WriteLine($"\nСТАТИСТИКА:");
            Console.WriteLine($"  Всего найдено IP-адресов: {ipAddresses.Length}");
            if (ipAddresses.Length > 0)
            {
                Console.WriteLine($"  Уникальных IP-адресов: {GetUniqueCount(ipAddresses)}");
                Console.WriteLine("\n  Список найденных IP-адресов:");
                foreach (string ip in ipAddresses)
                {
                    Console.WriteLine($"    - {ip}");
                }
            }
        }
    }
    private static void ShowAnalysis()
    {
        Console.WriteLine("\nРЕЗУЛЬТАТЫ:");
        Console.WriteLine(new string('-', 40));
        string[] expectedValidIPs = {
            "192.168.1.1", "10.0.0.1", "172.16.254.1", "8.8.8.8",
            "127.0.0.1", "192.168.0.1", "10.0.0.254",
            "0.0.0.0", "255.255.255.255",
            "1.2.3.4", "11.22.33.44", "101.102.103.104",
            "203.0.113.195", "198.51.100.1", "203.0.113.254"
        };
        string[] expectedInvalidIPs = {
            "192.168.1.256", "121.121.121.121.2",
            "111.111.111.1111", "300.400.500.600",
            "010.010.010.010", "192.168.1.1a",
            "a192.168.1.1", "192.168.1.",
            ".192.168.1.1", "192.168.1.1."
        };
        if (File.Exists("output.txt"))
        {
            string[] foundIPs = File.ReadAllLines("output.txt");
            Console.WriteLine("Ожидаемые валидные IP, которые должны быть найдены:");
            int validFound = 0;
            foreach (string ip in expectedValidIPs)
            {
                bool found = Array.IndexOf(foundIPs, ip) >= 0;
                Console.WriteLine($"  {(found ? "✓" : "✗")} {ip}: {(found ? "найден" : "потеря потерь")}");
                if (found) validFound++;
            }
            Console.WriteLine($"\n  Найдено корректных IP: {validFound} из {expectedValidIPs.Length}");
            Console.WriteLine("\nОжидаемые невалидные IP, которые НЕ должны быть найдены:");
            int invalidFound = 0;
            foreach (string ip in expectedInvalidIPs)
            {
                bool found = Array.IndexOf(foundIPs, ip) >= 0;
                Console.WriteLine($"  {(found ? "✗" : "✓")} {ip}: {(found ? "ОШИБОЧНО НАЙДЕН" : "НЕ НАЙДЕН (корректно)")}");
                if (found) invalidFound++;
            }
            Console.WriteLine($"\n  Ошибочно найдено невалидных IP: {invalidFound}");
            double accuracy = (double)validFound / expectedValidIPs.Length * 100;
            Console.WriteLine($"\nработа окончена на: {accuracy:F1}%");
        }
    }
    private static int GetUniqueCount(string[] array)
    {
        var unique = new HashSet<string>();
        foreach (string item in array)
        {
            unique.Add(item);
        }
        return unique.Count;
    }
}