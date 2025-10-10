using System;
public struct ComplexNumber
{
    private double real;
    private double imaginary;
    public ComplexNumber(double real, double imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;
    }
    public static ComplexNumber Create(double real, double imaginary)
    {
        return new ComplexNumber(real, imaginary);
    }
    public ComplexNumber Add(ComplexNumber other)
    {
        return new ComplexNumber(this.real + other.real, this.imaginary + other.imaginary);
    }
    public ComplexNumber Subtract(ComplexNumber other)
    {
        return new ComplexNumber(this.real - other.real, this.imaginary - other.imaginary);
    }
    public ComplexNumber Multiply(ComplexNumber other)
    {
        double newReal = this.real * other.real - this.imaginary * other.imaginary;
        double newImaginary = this.real * other.imaginary + this.imaginary * other.real;
        return new ComplexNumber(newReal, newImaginary);
    }
    public ComplexNumber Divide(ComplexNumber other)
    {
        double denominator = other.real * other.real + other.imaginary * other.imaginary;
        if (denominator == 0)
        {
            throw new DivideByZeroException("Деление на нулевое комплексное число");
        }

        double newReal = (this.real * other.real + this.imaginary * other.imaginary) / denominator;
        double newImaginary = (this.imaginary * other.real - this.real * other.imaginary) / denominator;
        return new ComplexNumber(newReal, newImaginary);
    }
    public double Modul()
    {
        return Math.Sqrt(real * real + imaginary * imaginary);
    }
    public double Argument()
    {
        if (real == 0 && imaginary == 0)
        {
            return 0;
        }
        return Math.Atan2(imaginary, real);
    }
    public double GetReal()
    {
        return real;
    }
    public double GetImaginary()
    {
        return imaginary;
    }
    public void Print()
    {
        if (imaginary >= 0)
        {
            Console.WriteLine($"{real:F2} + {imaginary:F2}i");
        }
        else
        {
            Console.WriteLine($"{real:F2} - {Math.Abs(imaginary):F2}i");
        }
    }
    public override string ToString()
    {
        if (imaginary >= 0)
        {
            return $"{real:F2} + {imaginary:F2}i";
        }
        else
        {
            return $"{real:F2} - {Math.Abs(imaginary):F2}i";
        }
    }
}
class Program
{
    static void Main()
    {
        ComplexNumber currentNumber = new ComplexNumber(0, 0); 
        bool exitProgram = false;
        Console.WriteLine("Калькулятор");
        Console.WriteLine("Текущее число: " + currentNumber);
        while (!exitProgram)
        {
            DisplayMenu();
            Console.Write("Выберите операцию: ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();
            try
            {
                switch (choice)
                {
                    case '1': 
                        currentNumber = CreateNewNumber();
                        break;
                    case '2': 
                        currentNumber = PerformOperation(currentNumber, "сложения", (a, b) => a.Add(b));
                        break;
                    case '3': 
                        currentNumber = PerformOperation(currentNumber, "вычитания", (a, b) => a.Subtract(b));
                        break;
                    case '4': 
                        currentNumber = PerformOperation(currentNumber, "умножения", (a, b) => a.Multiply(b));
                        break;
                    case '5': 
                        currentNumber = PerformOperation(currentNumber, "деления", (a, b) => a.Divide(b));
                        break;
                    case '6': 
                        Console.WriteLine($"Модуль числа: {currentNumber.Modul():F2}");
                        break;
                    case '7': 
                        Console.WriteLine($"Аргумент числа: {currentNumber.Argument():F2} радиан");
                        break;
                    case '8': 
                        Console.WriteLine($"Вещественная часть: {currentNumber.GetReal():F2}");
                        break;
                    case '9': 
                        Console.WriteLine($"Мнимая часть: {currentNumber.GetImaginary():F2}");
                        break;
                    case 'p': 
                    case 'P':
                        Console.Write("Текущее число: ");
                        currentNumber.Print();
                        break;
                    case 'q': 
                    case 'Q':
                        exitProgram = true;
                        Console.WriteLine("Выход");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
    static void DisplayMenu()
    {
        Console.WriteLine("=== Меню операций ===");
        Console.WriteLine("1 - Создать новое комплексное число");
        Console.WriteLine("2 - Сложение");
        Console.WriteLine("3 - Вычитание");
        Console.WriteLine("4 - Умножение");
        Console.WriteLine("5 - Деление");
        Console.WriteLine("6 - Модуль");
        Console.WriteLine("7 - Аргумент");
        Console.WriteLine("8 - Вещественная часть");
        Console.WriteLine("9 - Мнимая часть");
        Console.WriteLine("P - Вывод текущего числа");
        Console.WriteLine("Q - Выход");
    }
    static ComplexNumber CreateNewNumber()
    {
        Console.Write("Введите вещественную часть: ");
        double real = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть: ");
        double imaginary = double.Parse(Console.ReadLine());
        ComplexNumber newNumber = ComplexNumber.Create(real, imaginary);
        Console.Write("Создано число: ");
        newNumber.Print();
        return newNumber;
    }
    static ComplexNumber PerformOperation(ComplexNumber current, string operationName, Func<ComplexNumber, ComplexNumber, ComplexNumber> operation)
    {
        Console.WriteLine($"Операция {operationName}");
        Console.Write("Введите вещественную часть второго числа: ");
        double real2 = double.Parse(Console.ReadLine());
        Console.Write("Введите мнимую часть второго числа: ");
        double imaginary2 = double.Parse(Console.ReadLine());
        ComplexNumber secondNumber = ComplexNumber.Create(real2, imaginary2);
        ComplexNumber result = operation(current, secondNumber);
        Console.Write("Результат: ");
        result.Print();
        return result;
    }
}