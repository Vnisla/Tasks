using System;
using System.IO;
class Program
{
    static void Main()
    {
        try
        {
            string[] inputLines = File.ReadAllLines("чтиво.txt");
            int matrixSize = int.Parse(inputLines[0]);
            double[,] metricMatrix = new double[matrixSize, matrixSize];
            int currentLine = 1;
            for (int row = 0; row < matrixSize; row++)
            {
                string[] matrixRow = inputLines[currentLine].Split(' ');
                for (int col = 0; col < matrixSize; col++)
                {
                    metricMatrix[row, col] = double.Parse(matrixRow[col]);
                }
                currentLine++;
            }
            //Чтение вектора
            double[] vector = new double[matrixSize];
            string[] vectorComponents = inputLines[currentLine].Split(' ');
            for (int i = 0; i < matrixSize; i++)
            {
                vector[i] = double.Parse(vectorComponents[i]);
            }
            //Проверка симметричности матрицы
            if (!IsMatrixSymmetric(metricMatrix, matrixSize))
            {
                Console.WriteLine("Ошибка: матрица G не симметрична");
                return;
            }
            //длина вектора
            double l = CalculateVectorNorm(metricMatrix, vector, matrixSize);

            Console.WriteLine($"Длина вектора: {l:F6}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
    static bool IsMatrixSymmetric(double[,] m, int size)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                if (Math.Abs(m[i, j] - m[j, i]) > 1e-10)
                {
                    return false;
                }
            }
        }
        return true;
    }
    static double CalculateVectorNorm(double[,] metric, double[] v, int size)
    {
        //Вычисление произведения матрицы
        double[] transformedVector = new double[size];
        for (int i = 0; i < size; i++)
        {
            transformedVector[i] = 0;
            for (int j = 0; j < size; j++)
            {
                transformedVector[i] += metric[i, j] * v[j];
            }
        }
        //Вычисление квадрата нормы 
        double squaredNorm = 0;
        for (int i = 0; i < size; i++)
        {
            squaredNorm += v[i] * transformedVector[i];
        }
        return Math.Sqrt(squaredNorm);
    }
}