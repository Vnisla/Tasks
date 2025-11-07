using System;
using System.Collections;
using System.Collections.Generic;
public class MyArrayList<T> : IEnumerable<T>
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
        Array.Copy(a, 0, elementData, 0, a.Length);
        size = a.Length;
    }
    public MyArrayList(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative");
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
        if (o is T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
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
        if (o is T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                {
                    RemoveAt(i);
                    return true;
                }
            }
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
        var result = new List<T>();
        for (int i = 0; i < size; i++)
        {
            if (Array.IndexOf(a, elementData[i]) >= 0)
            {
                result.Add(elementData[i]);
            }
        }
        Clear();
        foreach (var item in result)
        {
            Add(item);
        }
    }
    public int Size()
    {
        return size;
    }
    public T[] ToArray()
    {
        T[] result = new T[size];
        Array.Copy(elementData, 0, result, 0, size);
        return result;
    }
    public T[] ToArray(T[] a)
    {
        if (a == null)
            return ToArray();
        if (a.Length < size)
            return ToArray();
        Array.Copy(elementData, 0, a, 0, size);
        if (a.Length > size)
            a[size] = default(T);
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
        EnsureCapacity(size + a.Length);
        if (index < size)
        {
            Array.Copy(elementData, index, elementData, index + a.Length, size - index);
        }
        Array.Copy(a, 0, elementData, index, a.Length);
        size += a.Length;
    }
    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        return elementData[index];
    }
    public int IndexOf(object o)
    {
        if (o is T item)
        {
            for (int i = 0; i < size; i++)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                    return i;
            }
        }
        return -1;
    }
    public int LastIndexOf(object o)
    {
        if (o is T item)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                    return i;
            }
        }
        return -1;
    }
    public T Remove(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T removed = elementData[index];
        int numMoved = size - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[size - 1] = default(T);
        size--;
        return removed;
    }
    public T Set(int index, T e)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        T oldValue = elementData[index];
        elementData[index] = e;
        return oldValue;
    }
    public MyArrayList<T> SubList(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex > size || fromIndex > toIndex)
            throw new ArgumentOutOfRangeException("Invalid index range");
        int subListSize = toIndex - fromIndex;
        T[] subArray = new T[subListSize];
        Array.Copy(elementData, fromIndex, subArray, 0, subListSize);
        return new MyArrayList<T>(subArray);
    }
    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity = elementData.Length * 3 / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, 0, newArray, 0, size);
            elementData = newArray;
        }
    }
    private void RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        int numMoved = size - index - 1;
        if (numMoved > 0)
        {
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        }
        elementData[size - 1] = default(T);
        size--;
    }
    public int Capacity => elementData.Length;
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < size; i++)
        {
            yield return elementData[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public T this[int index]
    {
        get => Get(index);
        set => Set(index, value);
    }
    public override string ToString()
    {
        if (size == 0)
            return "MyArrayList[]";
        var result = "MyArrayList[";
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
class Program
{
    static void Main()
    {
        Console.WriteLine(" MyArrayList");
        var list1 = new MyArrayList<int>();
        var list2 = new MyArrayList<int>(new int[] { 1, 2, 3, 4, 5 });
        var list3 = new MyArrayList<int>(20);
        Console.WriteLine($"list1 (пустой): {list1}");
        Console.WriteLine($"list2 (из массива): {list2}");
        Console.WriteLine($"list3 (ёмкость 20): {list3}");
        Console.WriteLine();
        TestBasicOperations();
        Console.WriteLine();
        TestIndexOperations();
        Console.WriteLine();
        TestBulkOperations();
    }
    static void TestBasicOperations()
    {
        Console.WriteLine(" Тестирование основных операций");
        var list = new MyArrayList<string>();
        list.Add("Apple");
        list.Add("Banana");
        list.Add("Cherry");
        Console.WriteLine($"После добавления: {list}");
        Console.WriteLine($"Размер: {list.Size()}, Ёмкость: {list.Capacity}");
        Console.WriteLine($"Содержит 'Banana': {list.Contains("Banana")}");
        Console.WriteLine($"Содержит 'Orange': {list.Contains("Orange")}");
        list.Remove("Banana");
        Console.WriteLine($"После удаления 'Banana': {list}");
        list.Clear();
        Console.WriteLine($"После очистки: {list}");
        Console.WriteLine($"Пустой: {list.IsEmpty()}");
    }
    static void TestIndexOperations()
    {
        Console.WriteLine("Тестирование операций с индексами");
        var list = new MyArrayList<int>();
        list.Add(0, 10);
        list.Add(0, 5);  //В начало
        list.Add(2, 20); //В конец
        list.Add(1, 7);  //В середину
        Console.WriteLine($"После добавления по индексам: {list}");
        Console.WriteLine($"Элемент по индексу 1: {list.Get(1)}");
        list.Set(1, 8);
        Console.WriteLine($"После установки 8 на индекс 1: {list}");
        Console.WriteLine($"Индекс числа 10: {list.IndexOf(10)}");
        Console.WriteLine($"Последний индекс числа 10: {list.LastIndexOf(10)}");
        int removed = list.Remove(0);
        Console.WriteLine($"Удалён элемент {removed}: {list}");
        var subList = list.SubList(1, 3);
        Console.WriteLine($"Подсписок [1,3): {subList}");
    }
    static void TestBulkOperations()
    {
        Console.WriteLine(" Тестирование массовых операций");
        var list = new MyArrayList<int>();
        list.AddAll(new int[] { 1, 2, 3, 4, 5 });
        Console.WriteLine($"После AddAll: {list}");
        list.AddAll(2, new int[] { 10, 11, 12 });
        Console.WriteLine($"После AddAll по индексу 2: {list}");
        Console.WriteLine($"Содержит все [1,2,3]: {list.ContainsAll(new int[] { 1, 2, 3 })}");
        Console.WriteLine($"Содержит все [1,6,3]: {list.ContainsAll(new int[] { 1, 6, 3 })}");
        list.RemoveAll(new int[] { 10, 11 });
        Console.WriteLine($"После RemoveAll [10,11]: {list}");
        list.RetainAll(new int[] { 1, 2, 12, 4 });
        Console.WriteLine($"После RetainAll [1,2,12,4]: {list}");
        int[] array1 = list.ToArray();
        int[] array2 = new int[10];
        list.ToArray(array2);
        Console.WriteLine($"ToArray(): [{string.Join(", ", array1)}]");
        Console.WriteLine($"ToArray(массив): [{string.Join(", ", array2)}]");
        Console.WriteLine("\nТестирование автоматического расширения");
        var bigList = new MyArrayList<int>();
        Console.WriteLine($"Начальная ёмкость: {bigList.Capacity}");
        for (int i = 0; i < 50; i++)
        {
            bigList.Add(i);
            if (i == 10 || i == 15 || i == 22 || i == 33 || i == 49)
            {
                Console.WriteLine($"После добавления {i + 1} элементов: размер={bigList.Size}, ёмкость={bigList.Capacity}");
            }
        }
        Console.WriteLine("\nТестирование foreach");
        Console.Write("Элементы через foreach: ");
        foreach (var item in bigList)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}