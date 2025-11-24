using System;
public class MyVector<T>
{
    protected T[] elementData;
    protected int elementCount;
    protected int capacityIncrement;
    public MyVector(int initialCapacity = 10, int capacityIncrement = 0)
    {
        if (initialCapacity < 0)
            throw new ArgumentException("Начальная емкость не может быть отрицательной");
        this.elementData = new T[initialCapacity];
        this.elementCount = 0;
        this.capacityIncrement = capacityIncrement;
    }
    public MyVector(T[] a) : this(a.Length)
    {
        Array.Copy(a, elementData, a.Length);
        elementCount = a.Length;
    }
    public virtual void Add(T e)
    {
        EnsureCapacity(elementCount + 1);
        elementData[elementCount++] = e;
    }
    protected void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > elementData.Length)
        {
            int newCapacity;
            if (capacityIncrement > 0)
                newCapacity = elementData.Length + capacityIncrement;
            else
                newCapacity = elementData.Length * 2;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            T[] newArray = new T[newCapacity];
            Array.Copy(elementData, newArray, elementCount);
            elementData = newArray;
        }
    }
    public virtual T Get(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new IndexOutOfRangeException("Индекс вне диапазона");
        return elementData[index];
    }
    public virtual T Set(int index, T element)
    {
        if (index < 0 || index >= elementCount)
            throw new IndexOutOfRangeException("Индекс вне диапазона");
        T oldValue = elementData[index];
        elementData[index] = element;
        return oldValue;
    }
    public virtual int Size()
    {
        return elementCount;
    }
    public virtual bool IsEmpty()
    {
        return elementCount == 0;
    }

    public virtual T Remove(int index)
    {
        if (index < 0 || index >= elementCount)
            throw new IndexOutOfRangeException("Индекс вне диапазона");
        T oldValue = elementData[index];
        int numMoved = elementCount - index - 1;
        if (numMoved > 0)
            Array.Copy(elementData, index + 1, elementData, index, numMoved);
        elementData[--elementCount] = default(T);
        return oldValue;
    }
    public virtual void Add(int index, T element)
    {
        if (index < 0 || index > elementCount)
            throw new IndexOutOfRangeException("Индекс вне диапазона");
        EnsureCapacity(elementCount + 1);
        Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
        elementData[index] = element;
        elementCount++;
    }
    public virtual T FirstElement()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Вектор пуст");
        return elementData[0];
    }
    public virtual T LastElement()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Вектор пуст");
        return elementData[elementCount - 1];
    }
    public virtual void RemoveElementAt(int pos)
    {
        Remove(pos);
    }
}
public class MyStack<T> : MyVector<T>
{
    public MyStack() : base() { }
    public MyStack(int initialCapacity) : base(initialCapacity) { }
    public MyStack(T[] a) : base(a) { }
    public void Push(T item)
    {
        Add(item);
    }
    public T Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Стек пуст");
        return Remove(elementCount - 1);
    }
    public T Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Стек пуст");
        return LastElement();
    }
    public bool Empty()
    {
        return IsEmpty();
    }
    public int Search(T item)
    {
        for (int i = elementCount - 1, depth = 1; i >= 0; i--, depth++)
        {
            if (EqualityComparer<T>.Default.Equals(elementData[i], item))
                return depth;
        }
        return -1;
    }
}
class Program
{
    static void Main()
    {
        MyStack<int> stack = new MyStack<int>();
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        Console.WriteLine($"Верхний элемент: {stack.Peek()}"); 
        Console.WriteLine($"Глубина элемента 20: {stack.Search(20)}"); 
        Console.WriteLine($"Извлеченный элемент: {stack.Pop()}"); 
        Console.WriteLine($"Извлеченный элемент: {stack.Pop()}"); 
        Console.WriteLine($"Стек пуст: {stack.Empty()}"); 
    }
}
