using System;
using System.IO;
using System.Text;
public class MyArrayList<T>
{
    private T[] elementData;
    private int size;
    public MyArrayList()
    {
        elementData = new T[10];
        size = 0;
    }
    public MyArrayList(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        elementData = new T[a.Length];
        Array.Copy(a, elementData, a.Length);
        size = a.Length;
    }
    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentException("Емкость не может быть отрицательной");
        elementData = new T[capacity];
        size = 0;
    }
    public void Add(T e)
    {
        EnsureCapacity(size + 1);
        elementData[size] = e;
        size++;
    }
    public void AddAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        EnsureCapacity(size + a.Length);
        Array.Copy(a, 0, elementData, size, a.Length);
        size += a.Length;
    }
    public void Clear()
    {
        for (int i = 0; i < size; i++)
        {
            elementData[i] = default(T);
        }
        size = 0;
    }
    public bool Contains(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < size; i++)
            {
                if (elementData[i] == null)
                    return true;
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
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
    public bool IsEmpty()
    {
        return size == 0;
    }
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
    public bool RemoveAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        bool modified = false;
        for (int i = size - 1; i >= 0; i--)
        {
            foreach (var item in a)
            {
                if ((item == null && elementData[i] == null) ||
                    (item != null && item.Equals(elementData[i])))
                {
                    RemoveAt(i);
                    modified = true;
                    break;
                }
            }
        }
        return modified;
    }
    public bool RetainAll(T[] a)
    {
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        bool modified = false;
        for (int i = size - 1; i >= 0; i--)
        {
            bool found = false;
            foreach (var item in a)
            {
                if ((item == null && elementData[i] == null) ||
                    (item != null && item.Equals(elementData[i])))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                RemoveAt(i);
                modified = true;
            }
        }
        return modified;
    }
    public int Size()
    {
        return size;
    }
    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(elementData, result, size);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null)
            return ToArray();
        if (a.Length < size)
        {
            return ToArray();
        }
        Array.Copy(elementData, a, size);
        if (a.Length > size)
        {
            a[size] = default(T);
        }
        return a;
    }
    public void Add(int index, T e)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity(size + 1);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + 1, size - index);
        }
        elementData[index] = e;
        size++;
    }
    public void AddAll(int index, T[] a)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (a == null)
            throw new ArgumentNullException(nameof(a));
        int numNew = a.Length;
        EnsureCapacity(size + numNew);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + numNew, size - index);
        }
        Array.Copy(a, 0, elementData, index, numNew);
        size += numNew;
    }
    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));

        return elementData[index];
    }
    public int IndexOf(object o)
    {
        if (o == null)
        {
            for (int i = 0; i < size; i++)
            {
                if (elementData[i] == null)
                    return i;
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
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
            for (int i = size - 1; i >= 0; i--)
            {
                if (elementData[i] == null)
                    return i;
            }
        }
        else
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (o.Equals(elementData[i]))
                    return i;
            }
        }
        return -1;
    }
    public T RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        int numMoved = size - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[--size] = default(T);
        return oldValue;
    }
    public T Set(int index, T e)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }
    public T[] SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException();
        int length = toIndex - fromIndex;
        T[] result = new T[length];
        Array.Copy(elementData, fromIndex, result, 0, length);
        return result;
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, size);
            elementData = newArray;
        }
    }
    public override string ToString()
    {
        if (size == 0)
            return "[]";
        string result = "[";
        for (int i = 0; i < size; i++)
        {
            result += elementData[i];
            if (i < size - 1)
                result += ", ";
        }
        result += "]";
        return result;
    }
}
public class TagProcessor
{
    private static MyArrayList<string> ExtractTagsFromLine(string line)
    {
        MyArrayList<string> tags = new MyArrayList<string>();
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '<')
            {
                int start = i;
                int end = line.IndexOf('>', i);
                if (end != -1)
                {
                    string tag = line.Substring(start, end - start + 1);
                    if (IsValidTag(tag))
                    {
                        tags.Add(tag);
                    }
                    i = end; 
                }
            }
        }
        return tags;
    }
    private static bool IsValidTag(string tag)
    {
        if (tag.Length < 3) 
            return false;
        int startIndex = 1;
        if (tag[1] == '/')
        {
            startIndex = 2;
        }
        if (startIndex >= tag.Length - 1 || !char.IsLetter(tag[startIndex]))
            return false;
        for (int i = startIndex; i < tag.Length - 1; i++)
        {
            if (!char.IsLetterOrDigit(tag[i]))
                return false;
        }
        return true;
    }
    private static string NormalizeTag(string tag)
    {
        string normalized = tag.ToLower();
        normalized = normalized.Replace("/", "");
        return normalized;
    }
    public static MyArrayList<string> RemoveDuplicateTags(MyArrayList<string> tags)
    {
        MyArrayList<string> uniqueTags = new MyArrayList<string>();
        MyArrayList<string> normalizedTags = new MyArrayList<string>();
        for (int i = 0; i < tags.Size(); i++)
        {
            string currentTag = tags.Get(i);
            string normalized = NormalizeTag(currentTag);
            if (!normalizedTags.Contains(normalized))
            {
                normalizedTags.Add(normalized);
                uniqueTags.Add(currentTag);
            }
        }
        return uniqueTags;
    }
    public static void ProcessFile(string inputFile)
    {
        try
        {
            MyArrayList<string> allTags = new MyArrayList<string>();
            string[] lines = File.ReadAllLines(inputFile);
            foreach (string line in lines)
            {
                MyArrayList<string> lineTags = ExtractTagsFromLine(line);
                for (int i = 0; i < lineTags.Size(); i++)
                {
                    allTags.Add(lineTags.Get(i));
                }
            }
            Console.WriteLine("Все найденные теги:");
            for (int i = 0; i < allTags.Size(); i++)
            {
                Console.WriteLine(allTags.Get(i));
            }
            MyArrayList<string> uniqueTags = RemoveDuplicateTags(allTags);
            Console.WriteLine("\nУникальные теги (без дубликатов):");
            for (int i = 0; i < uniqueTags.Size(); i++)
            {
                Console.WriteLine(uniqueTags.Get(i));
            }
            Console.WriteLine($"\nВсего найдено тегов: {allTags.Size()}");
            Console.WriteLine($"Уникальных тегов: {uniqueTags.Size()}");
            SaveResultsToFile(uniqueTags, "output.txt");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {inputFile} не найден!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
        }
    }
    private static void SaveResultsToFile(MyArrayList<string> tags, string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("Уникальные теги:");
                for (int i = 0; i < tags.Size(); i++)
                {
                    writer.WriteLine(tags.Get(i));
                }
                writer.WriteLine($"\nВсего уникальных тегов: {tags.Size()}");
            }
            Console.WriteLine($"Результаты сохранены в файл: {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
        }
    }
}
class Program
{
    static void Main()
    {
        CreateTestFile();
        TagProcessor.ProcessFile("input.txt");
        Console.WriteLine("\n--- Демонстрация MyArrayList ---");
        MyArrayList<string> testList = new MyArrayList<string>();
        testList.Add("<html>");
        testList.Add("</H1>");
        testList.Add("<Privet>");
        testList.Add("<html>");
        testList.Add("</Html>");
        Console.WriteLine("Исходный список: " + testList);
        MyArrayList<string> unique = TagProcessor.RemoveDuplicateTags(testList);
        Console.WriteLine("После удаления дубликатов: " + unique);
    }
    static void CreateTestFile()
    {
        string[] testContent = {
            "<html><head><title>Test</title></head>",
            "<body><H1>Hello</H1><div>Content</div>",
            "<Privet>Russian text</PrivetKEK>",
            "<html><body>Duplicate tags</body></html>",
            "<div123>Invalid tag</div123>",
            "</Html>Closing tag</Html>",
            "<script>alert('test');</script>"
        };
        File.WriteAllLines("input.txt", testContent);
        Console.WriteLine("Создан тестовый файл input.txt");
    }
}