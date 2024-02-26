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
                throw new DivideByZeroException("���������� ������ �� ����");
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
                throw new ArgumentException("���������� ��������� ���������� ������ �� �������������� �����");
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
                throw new DivideByZeroException("���������� ������ �� ����");
            }

            return 1 / a;
        }

        public double Factorial(double a)
        {
            if (a < 0)
            {
                throw new ArgumentException("��������� ����� ��������� ������ ��� ��������������� ����� �����");
            }

            if (a != Math.Round(a))
            {
                throw new ArgumentException("��������� ����� ������� ������ � ����� �����");
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
                throw new ArgumentException("����������� �������� ��������� ������ ��� ������������� �����");
            }

            return Math.Log(x);
        }

        public double Log(double x, double newBase)
        {
            if (x <= 0 || newBase <= 0 || newBase == 1)
            {
                throw new ArgumentException("��������� ���������� ������ ��� ������������� ����� � ���� ��������� �� ������ 1");
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