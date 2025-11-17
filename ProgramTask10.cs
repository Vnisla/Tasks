using System;
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
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(" вектор MyVector");
        TestConstructors();
        TestBasicOperations();
        TestIndexOperations();
        TestSpecialOperations();
        Console.WriteLine("\nВсе тесты завершены успешно!");
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
    private static void TestConstructors()
    {
        Console.WriteLine("\n1. Тестирование конструкторов:");
        MyVector<int> vector1 = new MyVector<int>();
        Console.WriteLine($"Вектор по умолчанию: Count={vector1.Count}, Capacity={vector1.Capacity}");
        MyVector<string> vector2 = new MyVector<string>(20);
        Console.WriteLine($"Вектор с емкостью 20: Count={vector2.Count}, Capacity={vector2.Capacity}");
        MyVector<double> vector3 = new MyVector<double>(5, 10);
        Console.WriteLine($"Вектор (5,10): Count={vector3.Count}, Capacity={vector3.Capacity}");
        int[] array = { 1, 2, 3, 4, 5 };
        MyVector<int> vector4 = new MyVector<int>(array);
        Console.WriteLine($"Вектор из массива: Count={vector4.Count}, Capacity={vector4.Capacity}");
        Console.Write("Элементы: ");
        for (int i = 0; i < vector4.Count; i++)
        {
            Console.Write(vector4.Get(i) + " ");
        }
        Console.WriteLine();
    }
    private static void TestBasicOperations()
    {
        Console.WriteLine("\n2. Тестирование основных операций:");
        MyVector<string> vector = new MyVector<string>();
        vector.Add("Первый");
        vector.Add("Второй");
        vector.Add("Третий");
        Console.WriteLine($"После добавления 3 элементов: Count={vector.Count}");
        string[] newItems = { "Четвертый", "Пятый" };
        vector.AddAll(newItems);
        Console.WriteLine($"После AddAll: Count={vector.Count}");
        Console.WriteLine($"Содержит 'Второй': {vector.Contains("Второй")}");
        Console.WriteLine($"Содержит 'Десятый': {vector.Contains("Десятый")}");
        vector.Remove("Второй");
        Console.WriteLine($"После удаления 'Второй': Count={vector.Count}");
        Console.WriteLine($"Содержит 'Второй': {vector.Contains("Второй")}");
        string[] array = vector.ToArray();
        Console.Write("Элементы массива: ");
        foreach (string item in array)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
    private static void TestIndexOperations()
    {
        Console.WriteLine("\n3. Тестирование операций с индексами:");
        MyVector<int> vector = new MyVector<int>();
        for (int i = 1; i <= 5; i++)
        {
            vector.Add(i * 10);
        }
        Console.Write("Исходный вектор: ");
        for (int i = 0; i < vector.Count; i++)
        {
            Console.Write(vector.Get(i) + " ");
        }
        Console.WriteLine();
        vector.Add(2, 25);
        Console.Write("После добавления 25 на позицию 2: ");
        for (int i = 0; i < vector.Count; i++)
        {
            Console.Write(vector.Get(i) + " ");
        }
        Console.WriteLine();
        int oldValue = vector.Set(3, 35);
        Console.WriteLine($"Замена на позиции 3: {oldValue} -> 35");
        int removed = vector.RemoveAt(1);
        Console.WriteLine($"Удален элемент на позиции 1: {removed}");
        Console.Write("После удаления: ");
        for (int i = 0; i < vector.Count; i++)
        {
            Console.Write(vector.Get(i) + " ");
        }
        Console.WriteLine();
        int index = vector.IndexOf(35);
        Console.WriteLine($"Индекс элемента 35: {index}");
        Console.WriteLine($"Первый элемент: {vector.FirstElement()}");
        Console.WriteLine($"Последний элемент: {vector.LastElement()}");
    }
    private static void TestSpecialOperations()
    {
        Console.WriteLine("\n4. Тестирование специальных операций:");
        MyVector<int> vector = new MyVector<int>();
        for (int i = 1; i <= 10; i++)
        {
            vector.Add(i);
        }
        Console.Write("Исходный вектор: ");
        for (int i = 0; i < vector.Count; i++)
        {
            Console.Write(vector.Get(i) + " ");
        }
        Console.WriteLine();
        MyVector<int> subList = vector.SubList(2, 7);
        Console.Write("Подсписок [2-7): ");
        for (int i = 0; i < subList.Count; i++)
        {
            Console.Write(subList.Get(i) + " ");
        }
        Console.WriteLine();
        vector.RemoveRange(3, 6);
        Console.Write("После RemoveRange(3,6): ");
        for (int i = 0; i < vector.Count; i++)
        {
            Console.Write(vector.Get(i) + " ");
        }
        Console.WriteLine();
        vector.Clear();
        Console.WriteLine($"После Clear: Count={vector.Count}, IsEmpty={vector.IsEmpty()}");
    }

}
