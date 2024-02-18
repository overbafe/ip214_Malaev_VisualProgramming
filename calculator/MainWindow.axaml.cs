using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.ComponentModel;

namespace calculator
{
    public partial class MainWindow : Window
    {
        Calculator calculator;
        public MainWindow()
        {
            InitializeComponent();
            historyLabel.Content = "";
            calculator = new Calculator();
        }

        Operations operation = Operations.none;
        bool errorHappened = false;

        private void numberButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isFractional = currentString?.Contains('.');
            bool? isNegative = currentString?.Contains('-');

            if (isFractional == false && isNegative == false && currentString?.Length >= 16 || //Если число не содержит десятичной точки и знака минус, и его длина больше или равна 16, то ограничение достигнуто.
                (isFractional ^ isNegative) == true && currentString?.Length >= 17 ||          //Если число содержит либо десятичную точку, либо знак минус, и его длина больше или равна 17, то ограничение достигнуто.
                    isFractional == true && isNegative == true && currentString?.Length >= 18) //Если число содержит и десятичную точку, и знак минус, и его длина больше или равна 18, то ограничение достигнуто.
            {
            }
            else if (currentString?.Length == 1 && Convert.ToDouble(currentString) == 0)
            {
                currentLabel.Content = (sender as Button)?.Content;
            }
            else
            {
                string? senderString = ((sender as Button)?.Content as string);
                currentLabel.Content = $"{currentString}{senderString}";
            }

            currentLabelContentChanged();
        }

        private void eraseButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');

            //explicit bool comprasion cause of nullable bool
            if (isNegative == false && currentString?.Length <= 1 || isNegative == true && currentString?.Length <= 2)
            {
                currentLabel.Content = "0";
            }
            else
            {
                currentLabel.Content = $"{currentString?[..Convert.ToInt16(currentString?.Length - 1)]}";
            }

            currentLabelContentChanged();
        }
        private void clearEntryButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            currentLabel.Content = "0";
            currentLabelContentChanged();
        }
        private void clearButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            clear();
        }
        private void clear()
        {
            operation = Operations.none;
            currentLabel.Content = "0";
            historyLabel.Content = "";
            currentLabelContentChanged();
        }
        private void comaButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isFractional = currentString?.Contains(',');

            if (isFractional == true)
            {
                return;
            }

            currentLabel.Content = $"{currentLabel.Content as string},";

            currentLabelContentChanged();
        }
        private void negateButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            if (currentString?.Length == 1 && Convert.ToDouble(currentString) == 0)
            {
                return;
            }

            bool? isNegative = currentString?.StartsWith('-');

            if (isNegative == true)
            {
                currentLabel.Content = $"{(currentLabel.Content as string)?.TrimStart('-')}";
            }
            else
            {
                currentLabel.Content = $"-{currentLabel.Content as string}";
            }


            currentLabelContentChanged();
        }
        private void squareButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try
            {
                double? result = calculator.Square(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                updateLabels(@$"{currentString}²", result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void squareRootButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try
            {
                double? result = calculator.SquareRoot(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                updateLabels(@$"sqrt({currentString})", result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void percentButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);

            try
            {
                double? result = calculator.Percentage(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                updateLabels(result?.ToString(), result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void reciprocalButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Reciprocal(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"{1}/({currentString})" : $"{1}/{currentString}";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void factorialButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Factorial(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"{currentString}!" : $"{currentString}!";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void logButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Log(double.Parse(currentString), 2);

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"log({currentString})" : $"log({currentString})";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void lnButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Ln(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"ln({currentString})" : $"ln({currentString})";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void sinButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Sin(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"sin({currentString})" : $"sin({currentString})";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void cosButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Cos(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"cos({currentString})" : $"cos({currentString})";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void tgButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            bool? isNegative = currentString?.StartsWith('-');
            double? result = null;

            try
            {
                result = calculator.Tan(double.Parse(currentString));

                if (double.IsNaN(result.Value) || double.IsInfinity(result.Value))
                {
                    OnError();
                    return;
                }

                string rightOperand = isNegative == true ? $"tg({currentString})" : $"tg({currentString})";

                updateLabels(rightOperand, result?.ToString());
            }
            catch (Exception)
            {
                OnError();
                return;
            }
        }
        private void operationButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Content as string);
            string? senderString = (sender as Button)?.Content as string;

            Operations prevOperation = operation;

            operation = StringToOperations(senderString);

            double result = double.MinValue;

            try
            {
                if (prevOperation is not Operations.none)
                {
                    string? left = (historyLabel.Content as string)?.Split(' ')[0];
                    string? right = currentLabel.Content as string;
                    double leftPart = Convert.ToDouble(left);
                    double rightPart = Convert.ToDouble(right);
                    switch (prevOperation)
                    {
                        case Operations.plus:
                            {
                                result = calculator.Add(leftPart,rightPart);
                                break;
                            }
                        case Operations.minus:
                            {
                                result = calculator.Subtract(leftPart,rightPart);
                                break;
                            }
                        case Operations.times:
                            {
                                result = calculator.Multiply(leftPart,rightPart);
                                break;
                            }
                        case Operations.divide:
                            {
                                result = calculator.Divide(leftPart, rightPart);
                                break;
                            }
                        default:
                            {
                                historyLabel.Content = $"{currentString} {senderString}";
                                currentLabel.Content = "0";
                                currentLabelContentChanged();
                                return;
                            }
                    }
                }
                else
                {
                    historyLabel.Content = $"{currentString} {senderString}";
                    currentLabel.Content = "0";
                    currentLabelContentChanged();
                    return;
                }
            }
            catch (FormatException ex)
            {
                operation = prevOperation;
                historyLabel.Content = currentLabel.Content as string;
                operationButton_OnClick(sender, args);
                return;
            }
            catch (Exception ex)
            {
                OnError();
                return;
            }

            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                OnError();
                return;
            }


            if (operation != Operations.equal)
            {
                historyLabel.Content = $"{result} {senderString}";
                currentLabel.Content = "0";
            }
            else if (prevOperation == Operations.equal)
            {
                clear();
            }
            else
            {
                writeRightHistoryOperand($" {currentString} =");
                currentLabel.Content = result.ToString();
            }

            currentLabelContentChanged();
        }

        private void currentLabelContentChanged()
        {
            currentLabel.FontSize = (currentLabel.Content as string)?.Length switch
            {
                < 8 => 55,
                >= 8 and < 10 => 42,
                >= 10 and < 13 => 35,
                >= 13 and < 15 => 29,
                >= 15 => 26,
                null => 55
            };
        }
        enum Operations
        {
            none = 0,
            plus,
            minus,
            times,
            divide,
            equal
        }
        void writeRightHistoryOperand(string rightOperand)
        {
            string? historyString = (historyLabel.Content as string);
            string[]? historyStringArr = historyString?.Split(' ');

            if (historyStringArr?.Length < 3)
            {
                historyString += $"{rightOperand}";
            }
            else
            {
                historyStringArr[2] = $"{rightOperand}";
                historyString = $"{historyStringArr[0]} {historyStringArr[1]} {historyStringArr[2]}";
            }

            historyLabel.Content = historyString;
        }
        void OnErrorSkip()
        {
            errorHappened = false;
            clear();
            currentLabel.Foreground = Brushes.Black;
        }
        void OnError()
        {
            clear();
            errorHappened = true;
            currentLabel.Content = "ОШИБКА";
            currentLabel.Foreground = Brushes.Red;
        }
        void updateLabels(string rightOperand, string result)
        {
            if (operation != Operations.none && operation != Operations.equal)
            {
                writeRightHistoryOperand(@$" {rightOperand}");
            }
            else
            {
                historyLabel.Content = @$"{rightOperand}";
            }

            currentLabel.Content = result;

            currentLabelContentChanged();
        }
        Operations StringToOperations(string s)
        {
            switch (s)
            {
                case "+":
                    return Operations.plus;
                case @"⨉":
                    return Operations.times;
                case "/":
                    return Operations.divide;
                case @"—":
                    return Operations.minus;
                case "=":
                    return Operations.equal;
                default:
                    return Operations.none;
            }
        }
    }

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
