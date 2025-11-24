using System;
using System.Collections.Generic;
using System.Globalization;
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
public class ReversePolishNotation
{
    private MyStack<double> numberStack;
    private MyStack<string> operatorStack;
    private Dictionary<string, double> variables;
    public ReversePolishNotation()
    {
        numberStack = new MyStack<double>();
        operatorStack = new MyStack<string>();
        variables = new Dictionary<string, double>();
    }
    public double Evaluate(string expression, Dictionary<string, double> vars = null)
    {
        if (vars != null)
            variables = vars;

        string rpn = ConvertToRPN(expression);
        return EvaluateRPN(rpn);
    }
    private string ConvertToRPN(string expression)
    {
        operatorStack = new MyStack<string>();
        List<string> output = new List<string>();
        string[] tokens = Tokenize(expression);
        foreach (string token in tokens)
        {
            if (IsNumber(token))
            {
                output.Add(token);
            }
            else if (IsVariable(token))
            {
                output.Add(token);
            }
            else if (IsFunction(token))
            {
                operatorStack.Push(token);
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                while (!operatorStack.Empty() && operatorStack.Peek() != "(")
                {
                    output.Add(operatorStack.Pop());
                }
                if (!operatorStack.Empty() && operatorStack.Peek() == "(")
                    operatorStack.Pop();

                if (!operatorStack.Empty() && IsFunction(operatorStack.Peek()))
                {
                    output.Add(operatorStack.Pop());
                }
            }
            else if (IsOperator(token))
            {
                while (!operatorStack.Empty() && GetPriority(operatorStack.Peek()) >= GetPriority(token))
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
        }
        while (!operatorStack.Empty())
        {
            output.Add(operatorStack.Pop());
        }
        return string.Join(" ", output);
    }
    private double EvaluateRPN(string rpnExpression)
    {
        numberStack = new MyStack<double>();
        string[] tokens = rpnExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (string token in tokens)
        {
            if (IsNumber(token))
            {
                numberStack.Push(double.Parse(token, CultureInfo.InvariantCulture));
            }
            else if (IsVariable(token))
            {
                if (variables.ContainsKey(token))
                    numberStack.Push(variables[token]);
                else
                    throw new ArgumentException($"Неизвестная переменная: {token}");
            }
            else if (IsOperator(token) || IsFunction(token))
            {
                ExecuteOperation(token);
            }
        }
        if (numberStack.Size() != 1)
            throw new InvalidOperationException("Некорректное выражение");
        return numberStack.Pop();
    }
    private void ExecuteOperation(string operation)
    {
        switch (operation)
        {
            case "+":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(a + b);
                    break;
                }
            case "-":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(a - b);
                    break;
                }
            case "*":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(a * b);
                    break;
                }
            case "/":
                {
                    double b = numberStack.Pop();
                    if (b == 0) throw new DivideByZeroException("Деление на ноль");
                    double a = numberStack.Pop();
                    numberStack.Push(a / b);
                    break;
                }
            case "^":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Pow(a, b));
                    break;
                }
            case "sqrt":
                {
                    double a = numberStack.Pop();
                    if (a < 0) throw new ArgumentException("Корень из отрицательного числа");
                    numberStack.Push(Math.Sqrt(a));
                    break;
                }
            case "abs":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Abs(a));
                    break;
                }
            case "sin":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Sin(a));
                    break;
                }
            case "cos":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Cos(a));
                    break;
                }
            case "tan":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Tan(a));
                    break;
                }
            case "ln":
                {
                    double a = numberStack.Pop();
                    if (a <= 0) throw new ArgumentException("Логарифм неположительного числа");
                    numberStack.Push(Math.Log(a));
                    break;
                }
            case "log":
                {
                    double a = numberStack.Pop();
                    if (a <= 0) throw new ArgumentException("Логарифм неположительного числа");
                    numberStack.Push(Math.Log10(a));
                    break;
                }
            case "exp":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Exp(a));
                    break;
                }
            case "floor":
                {
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Floor(a));
                    break;
                }
            case "min":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Min(a, b));
                    break;
                }
            case "max":
                {
                    double b = numberStack.Pop();
                    double a = numberStack.Pop();
                    numberStack.Push(Math.Max(a, b));
                    break;
                }
            case "%":
                {
                    double b = numberStack.Pop();
                    if (b == 0) throw new DivideByZeroException("Деление на ноль");
                    double a = numberStack.Pop();
                    numberStack.Push(a % b);
                    break;
                }
            default:
                throw new ArgumentException($"Неизвестная операция: {operation}");
        }
    }
    private string[] Tokenize(string expression)
    {
        List<string> tokens = new List<string>();
        string current = "";
        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];
            if (char.IsWhiteSpace(c))
            {
                if (!string.IsNullOrEmpty(current))
                {
                    tokens.Add(current);
                    current = "";
                }
                continue;
            }
            if (char.IsDigit(c) || c == '.')
            {
                current += c;
            }
            else if (char.IsLetter(c))
            {
                current += c;
            }
            else if (IsOperatorChar(c) || c == '(' || c == ')')
            {
                if (!string.IsNullOrEmpty(current))
                {
                    tokens.Add(current);
                    current = "";
                }
                tokens.Add(c.ToString());
            }
        }
        if (!string.IsNullOrEmpty(current))
            tokens.Add(current);
        return tokens.ToArray();
    }
    private bool IsNumber(string token)
    {
        return double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
    }
    private bool IsVariable(string token)
    {
        return !IsFunction(token) && char.IsLetter(token[0]);
    }
    private bool IsFunction(string token)
    {
        string[] functions = { "sqrt", "sin", "cos", "tan", "ln", "log", "exp", "abs", "floor", "min", "max" };
        return Array.Exists(functions, f => f == token);
    }
    private bool IsOperator(string token)
    {
        string[] operators = { "+", "-", "*", "/", "^", "%" };
        return Array.Exists(operators, op => op == token);
    }
    private bool IsOperatorChar(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == '%';
    }
    private int GetPriority(string op)
    {
        switch (op)
        {
            case "+":
            case "-":
                return 1;
            case "*":
            case "/":
            case "%":
                return 2;
            case "^":
                return 3;
            case "sqrt":
            case "sin":
            case "cos":
            case "tan":
            case "ln":
            case "log":
            case "exp":
            case "abs":
            case "floor":
                return 4;
            default:
                return 0;
        }
    }
}
class Program
{
    static void Main()
    {
        ReversePolishNotation rpn = new ReversePolishNotation();
        try
        {
            string expr1 = "3 + 4 * 2 / (1 - 5) ^ 2";
            double result1 = rpn.Evaluate(expr1);
            Console.WriteLine($"{expr1} = {result1}");
            var variables = new Dictionary<string, double>
            {
                { "a", 5 },
                { "b", 10 },
                { "c", 3 }
            };
            string expr2 = "a + b * c - sqrt(9)";
            double result2 = rpn.Evaluate(expr2, variables);
            Console.WriteLine($"{expr2} = {result2}");
            string expr3 = "sin(0) + cos(0)";
            double result3 = rpn.Evaluate(expr3);
            Console.WriteLine($"{expr3} = {result3}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
