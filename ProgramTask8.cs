using System;
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
class Program
{
    static void Main()
    {
        MyArrayList<int> list = new MyArrayList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        Console.WriteLine("После добавления: " + list);
        list.Add(1, 10);
        Console.WriteLine("После добавления по индексу 1: " + list);
        Console.WriteLine("Элемент по индексу 2: " + list.Get(2));
        int removed = list.RemoveAt(0);
        Console.WriteLine($"Удален элемент: {removed}, список: {list}");
        bool removedByValue = list.Remove(10);
        Console.WriteLine($"Удаление значения 10: {removedByValue}, список: {list}");
        Console.WriteLine("Содержит 3: " + list.Contains(3));
        Console.WriteLine("Содержит 5: " + list.Contains(5));
        Console.WriteLine("Размер списка: " + list.Size());
        Console.WriteLine("Пустой ли: " + list.IsEmpty());
        int[] array = list.ToArray();
        Console.WriteLine("Массив: [" + string.Join(", ", array) + "]");
    }
}
