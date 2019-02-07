using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCalcul
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool flEqually, fl = false;
        string firstOperand, secondOperand, lastOperation = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            // обработчик работает только для кнопок
            Button btn = e.Source as Button;
            if (btn == null)
                return;
            string value = btn.Content.ToString();
            // если это кнопка настроек
            if (btn.Name == "SetupButton")
            {
                
            }
            else
            {
                // обработчик работает только для кнопок у которых есть значение (контент)
                if (String.IsNullOrEmpty(value))
                    return;
                int ct = TextBoxMain.Text.Length;
                string str = "";
                switch (value)
                {
                    // сброс
                    case "C":
                        TextBoxMain.Text = "0";
                        LabelMain.Content = "";
                        secondOperand = "";
                        fl = false;
                        flEqually = false;
                        break;
                    // сброс текстового поля
                    case "CE":
                        TextBoxMain.Text = "0";
                        flEqually = false;
                        break;
                    // стираем последний символ текстового поля
                    case "f":
                        if (ct > 1)
                        { TextBoxMain.Text = TextBoxMain.Text.Substring(0, ct - 1); }
                        else
                        {
                            if (TextBoxMain.Text != "0")
                            {
                                TextBoxMain.Text = "0";
                            }
                        }
                        flEqually = false;
                        break;
                    // меняем знак на противоположный
                    case "±":
                        string s = TextBoxMain.Text.Substring(0, 1);
                        if (s == "-")
                        { TextBoxMain.Text = TextBoxMain.Text.Substring(1, ct - 1); }
                        else
                        {
                            if (ct > 1 || ct == 1 && s != "0")
                            { TextBoxMain.Text = "-" + TextBoxMain.Text; }
                        }
                        flEqually = false;
                        break;
                    // Отображаем вспомогательную информацию
                    case "+":
                    case "-":
                    case "×":
                    case "÷":
                        lastOperation = value;
                        LabelMain.Content = TextBoxMain.Text + " " + value;
                        fl = true;
                        flEqually = false;
                        break;
                    // Работа с процентами
                    case "%":
                        lastOperation = "";
                        if (LabelMain.Content.ToString().Length > 0)
                        {
                            str = LabelMain.Content.ToString().Replace("÷", "/");
                            str = str.Replace("×", "*");
                            str = str.Replace(",", ".");
                            Calculate(TextBoxMain.Text.Replace(",", ".") + "/100");
                            if (str.IndexOf("+") >= 0 || str.IndexOf("-") >= 0)
                            {
                                string strnew = str.Replace("+", "*");
                                strnew = strnew.Replace("-", "*");
                                Calculate(strnew + TextBoxMain.Text.Replace(",", "."));
                                Calculate(str + TextBoxMain.Text.Replace(",", "."));
                                LabelMain.Content = "";
                            }
                        }
                        else
                        {
                            TextBoxMain.Text = "0";
                            LabelMain.Content = "";
                        }
                        flEqually = false;
                        break;
                    // Извлекаем корень
                    case "√":
                        lastOperation = "";
                        LabelMain.Content = "√(" + TextBoxMain.Text + ")";
                        str = TextBoxMain.Text.Replace(",", ".");
                        Calculate("Sqrt(" + str + ")");
                        flEqually = false;
                        break;
                    // Возводим в квадрат
                    case "x²":
                        lastOperation = "";
                        LabelMain.Content = "sqr(" + TextBoxMain.Text + ")";
                        str = TextBoxMain.Text.Replace(",", ".");
                        Calculate(str + "*" + str);
                        flEqually = false;
                        break;
                    // Получаем обратное значение
                    case "1/x":
                        lastOperation = "";
                        LabelMain.Content = "1/(" + TextBoxMain.Text + ")";
                        str = TextBoxMain.Text.Replace(",", ".");
                        Calculate("1/" + str);
                        flEqually = false;
                        break;
                    // считаем выражение с помощью библиотеки NCalc
                    case "=":
                        if (!flEqually)
                        {
                            secondOperand = TextBoxMain.Text.Replace(",", ".");
                        }
                        else
                        {
                            if (lastOperation.Length > 0)
                            {
                                LabelMain.Content = TextBoxMain.Text.Replace(",", ".") + " " + lastOperation;
                            }
                        }
                        if (LabelMain.Content.ToString().Length > 0)
                        {
                            str = LabelMain.Content.ToString().Replace("÷", "/");
                            str = str.Replace("×", "*");
                            str = str.Replace(",", ".");
                            Calculate(str + secondOperand);
                        }
                        LabelMain.Content = "";
                        flEqually = true;
                        break;
                    // добавялем новое значение в текстовое поле 
                    default:
                        if (fl)
                        {
                            TextBoxMain.Text = "";
                            fl = false;
                        }
                        if (value == "0")
                        {
                            if (ct > 1 || ct == 1 && TextBoxMain.Text != "0")
                            { TextBoxMain.Text = TextBoxMain.Text + value; }
                        }
                        else
                        {
                            if (ct == 1 && TextBoxMain.Text == "0")
                            { TextBoxMain.Text = value == "," ? "0," : value; }
                            else
                            { TextBoxMain.Text = TextBoxMain.Text + value; }
                        }
                        flEqually = false;
                        break;
                }
            }
        }
        private void Calculate(string prm)
        {
            try
            {
                NCalc.Expression exp = new NCalc.Expression(prm);
                TextBoxMain.Text = exp.Evaluate().ToString();
                fl = true;
            }
            catch
            {
                TextBoxMain.Text = "Error";
            }
        }
    }
}
