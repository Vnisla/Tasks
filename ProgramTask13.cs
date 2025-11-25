  Код с вводом с консоли и обработкой переменных

```csharp
using System;
using System.Collections.Generic;
using System.Globalization;

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
        Console.WriteLine($"RPN: {rpn}");
        
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
                while (!operatorStack.Empty() && 
                       operatorStack.Peek() != "(" &&
                       GetPriority(operatorStack.Peek()) >= GetPriority(token))
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
            case "sqrt": case "sin": case "cos": case "tan":
            case "ln": case "log": case "exp": case "abs": case "floor":
            case "min": case "max":
                return 4;
            default:
                return 0;
        }
    }

    // Метод для извлечения переменных из выражения
    public List<string> ExtractVariables(string expression)
    {
        List<string> variablesList = new List<string>();
        string[] tokens = Tokenize(expression);
        
        foreach (string token in tokens)
        {
            if (IsVariable(token) && !variablesList.Contains(token))
            {
                variablesList.Add(token);
            }
        }
        
        return variablesList;
    }
}

// Класс MyStack (упрощенная версия)
public class MyStack<T>
{
    private List<T> items = new List<T>();
    
    public void Push(T item) => items.Add(item);
    
    public T Pop()
    {
        if (items.Count == 0) throw new InvalidOperationException("Стек пуст");
        T item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        return item;
    }
    
    public T Peek()
    {
        if (items.Count == 0) throw new InvalidOperationException("Стек пуст");
        return items[items.Count - 1];
    }
    
    public bool Empty() => items.Count == 0;
    
    public int Size() => items.Count;
}

// Главная программа с консольным вводом
class Program
{
    static void Main()
    {
        ReversePolishNotation rpn = new ReversePolishNotation();
        
        Console.WriteLine("=== Калькулятор обратной польской записи ===");
        Console.WriteLine("Поддерживаемые операции: +, -, *, /, ^, %, sqrt, sin, cos, tan, ln, log, exp, abs, floor, min, max");
        Console.WriteLine("Для выхода введите 'exit'");
        Console.WriteLine();
        
        while (true)
        {
            try
            {
                // Ввод выражения
                Console.Write("Введите математическое выражение: ");
                string expression = Console.ReadLine();
                
                if (expression.ToLower() == "exit")
                    break;
                    
                if (string.IsNullOrWhiteSpace(expression))
                    continue;
                
                // Извлечение переменных из выражения
                List<string> variablesInExpression = rpn.ExtractVariables(expression);
                
                Dictionary<string, double> variables = new Dictionary<string, double>();
                
                // Ввод значений переменных
                if (variablesInExpression.Count > 0)
                {
                    Console.WriteLine($"Найдены переменные: {string.Join(", ", variablesInExpression)}");
                    Console.WriteLine("Введите значения переменных:");
                    
                    foreach (string varName in variablesInExpression)
                    {
                        Console.Write($"{varName} = ");
                        string input = Console.ReadLine();
                        
                        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                        {
                            variables[varName] = value;
                        }
                        else
                        {
                            throw new ArgumentException($"Некорректное значение для переменной {varName}");
                        }
                    }
                    Console.WriteLine();
                }
                
                // Вычисление выражения
                double result = rpn.Evaluate(expression, variables);
                Console.WriteLine($"Результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
            Console.WriteLine(new string('-', 50));
        }
        
        Console.WriteLine("Программа завершена.");
    }
}
public List<string> ExtractVariables(string expression)
{
    List<string> variablesList = new List<string>();
    string[] tokens = Tokenize(expression);
    foreach (string token in tokens)
    {
        if (IsVariable(token) && !variablesList.Contains(token))
        {
            variablesList.Add(token);
        }
    }
    return variablesList;
}
