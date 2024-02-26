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
            historyLabel.Text = "";
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

            string? currentString = (currentLabel.Text); ;
            bool? isFractional = currentString?.Contains('.');
            bool? isNegative = currentString?.Contains('-');

            if (isFractional == false && isNegative == false && currentString?.Length >= 16 || //Если число не содержит десятичной точки и знака минус, и его длина больше или равна 16, то ограничение достигнуто.
                (isFractional ^ isNegative) == true && currentString?.Length >= 17 ||          //Если число содержит либо десятичную точку, либо знак минус, и его длина больше или равна 17, то ограничение достигнуто.
                    isFractional == true && isNegative == true && currentString?.Length >= 18) //Если число содержит и десятичную точку, и знак минус, и его длина больше или равна 18, то ограничение достигнуто.
            {
            }
            else if (currentString?.Length == 1 && Convert.ToDouble(currentString) == 0)
            {
                currentLabel.Text = (sender as Button)?.Content as string;
            }
            else
            {
                string? senderString = ((sender as Button)?.Content as string);
                currentLabel.Text = $"{currentString}{senderString}";
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

            string? currentString = (currentLabel.Text);
            bool? isNegative = currentString?.StartsWith('-');

            //explicit bool comprasion cause of nullable bool
            if (isNegative == false && currentString?.Length <= 1 || isNegative == true && currentString?.Length <= 2)
            {
                currentLabel.Text = "0";
            }
            else
            {
                currentLabel.Text = $"{currentString?[..Convert.ToInt16(currentString?.Length - 1)]}";
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

            currentLabel.Text = "0";
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
            currentLabel.Text = "0";
            historyLabel.Text = "";
            currentLabelContentChanged();
        }
        private void comaButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Text);
            bool? isFractional = currentString?.Contains(',');

            if (isFractional == true)
            {
                return;
            }

            currentLabel.Text = $"{currentLabel.Text},";

            currentLabelContentChanged();
        }
        private void negateButton_OnClick(object? sender, RoutedEventArgs args)
        {
            if (errorHappened)
            {
                OnErrorSkip();
                return;
            }

            string? currentString = (currentLabel.Text);

            if (currentString?.Length == 1 && Convert.ToDouble(currentString) == 0)
            {
                return;
            }

            bool? isNegative = currentString?.StartsWith('-');

            if (isNegative == true)
            {
                currentLabel.Text = $"{(currentLabel.Text)?.TrimStart('-')}";
            }
            else
            {
                currentLabel.Text = $"-{currentLabel.Text}";
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

            string? currentString = (currentLabel.Text);

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

            string? currentString = (currentLabel.Text);

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

            string? currentString = (currentLabel.Text);

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

            string? currentString = (currentLabel.Text  );
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

            string? currentString = (currentLabel.Text);
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

            string? currentString = (currentLabel.Text as string);
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

            string? currentString = (currentLabel.Text as string);
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

            string? currentString = (currentLabel.Text as string);
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

            string? currentString = (currentLabel.Text as string);
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

            string? currentString = (currentLabel.Text as string);
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

            string? currentString = (currentLabel.Text);
            string? senderString = (sender as Button)?.Content as string;

            Operations prevOperation = operation;

            operation = StringToOperations(senderString);

            double result = double.MinValue;

            try
            {
                if (prevOperation is not Operations.none)
                {
                    string? left = (historyLabel.Text)?.Split(' ')[0];
                    string? right = currentLabel.Text;
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
                                historyLabel.Text = $"{currentString} {senderString}";
                                currentLabel.Text = "0";
                                currentLabelContentChanged();
                                return;
                            }
                    }
                }
                else
                {
                    historyLabel.Text = $"{currentString} {senderString}";
                    currentLabel.Text = "0";
                    currentLabelContentChanged();
                    return;
                }
            }
            catch (FormatException ex)
            {
                operation = prevOperation;
                historyLabel.Text = currentLabel.Text;
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
                historyLabel.Text = $"{result} {senderString}";
                currentLabel.Text = "0";
            }
            else if (prevOperation == Operations.equal)
            {
                clear();
            }
            else
            {
                writeRightHistoryOperand($" {currentString} =");
                currentLabel.Text = result.ToString();
            }

            currentLabelContentChanged();
        }

        private void currentLabelContentChanged()
        {
            currentLabel.FontSize = (currentLabel.Text)?.Length switch
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
            string? historyString = (historyLabel.Text);
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

            historyLabel.Text = historyString;
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
            currentLabel.Text = "ОШИБКА";
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
                historyLabel.Text = @$"{rightOperand}";
            }

            currentLabel.Text = result;

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
}