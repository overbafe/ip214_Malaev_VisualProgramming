using System;

namespace calculator
{
    public class Calculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            return a * b;
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Невозможно делить на ноль");
            }

            return a / b;
        }

        public double Square(double a)
        {
            return a * a;
        }

        public double SquareRoot(double a)
        {
            if (a < 0)
            {
                throw new ArgumentException("Невозможно вычислить квадратный корень из отрицательного числа");
            }

            return Math.Sqrt(a);
        }

        public double Percentage(double a)
        {
            return a / 100;
        }

        public double Reciprocal(double a)
        {
            if (a == 0)
            {
                throw new DivideByZeroException("Невозможно делить на ноль");
            }

            return 1 / a;
        }

        public double Factorial(double a)
        {
            if (a < 0)
            {
                throw new ArgumentException("Факториал можно вычислить только для неотрицательных целых чисел");
            }

            if (a != Math.Round(a))
            {
                throw new ArgumentException("Факториал можно считать только у целых чисел");
            }

            if (a == 0)
            {
                return 1;
            }

            double result = 1;
            for (int i = 1; i <= a; i++)
            {
                result *= i;
            }

            return result;
        }

        public double Ln(double x)
        {
            if (x <= 0)
            {
                throw new ArgumentException("Натуральный логарифм определен только для положительных чисел");
            }

            return Math.Log(x);
        }

        public double Log(double x, double newBase)
        {
            if (x <= 0 || newBase <= 0 || newBase == 1)
            {
                throw new ArgumentException("Логарифмы определены только для положительных чисел и базы логарифма не равной 1");
            }

            return Math.Log(x, newBase);
        }

        public double Sin(double x)
        {
            return Math.Sin(x);
        }

        public double Cos(double x)
        {
            return Math.Cos(x);
        }

        public double Tan(double x)
        {
            return Math.Tan(x);
        }
    }
}