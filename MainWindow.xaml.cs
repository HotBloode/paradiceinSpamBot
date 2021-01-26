using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace paradiceinSpamBot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> сurrencyList = new List<string>();
        private int count = 5;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private Controller c;
        private void Addсurrency()
        {
            if (BTC.IsChecked.Value)
            {
                сurrencyList.Add(BTC.Name);
            }

            if (ETH.IsChecked.Value)
            {
                сurrencyList.Add(BTC.Name);
            }

            if (LTC.IsChecked.Value)
            {
                сurrencyList.Add(LTC.Name);
            }

            if (DOGE.IsChecked.Value)
            {
                сurrencyList.Add(DOGE.Name);
            }

            if (DASH.IsChecked.Value)
            {
                сurrencyList.Add(DASH.Name);
            }

            if (PRDC.IsChecked.Value)
            {
                сurrencyList.Add(PRDC.Name);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Addсurrency();
            if (сurrencyList.Count==0)
            {
                MessageBox.Show("At least one currency to choose");
            }
            else
            {
                c = new Controller(info, Convert.ToDouble(baseBet.Text),Convert.ToInt32(maxRand.Text),Convert.ToInt32(countB.Text), сurrencyList);

                if (c.Start())
                {
                    Start.IsEnabled = false;
                    Stop.IsEnabled = true;
                }
            }
            
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < 0;
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if ((sender as TextBox).IsEnabled)
                {
                    if (Convert.ToDouble((sender as TextBox).Text) < 50 )
                    {
                        MessageBox.Show("MaxRand should be more than 50");
                        maxRand.Text = "250";
                    }
                }
            }
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789 ,".IndexOf(e.Text) < 0;
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (baseBet.Text == "" || baseBet.Text == null || baseBet.Text == " ")
                {
                    MessageBox.Show("You must enter the baseBet");
                    baseBet.Text = "0,00000001";
                }
                else
                {
                    if (Convert.ToDouble(baseBet.Text) < 0.00000001)
                    {
                        MessageBox.Show("baseBet must be more then 0,00000001");
                        baseBet.Text = "0,00000001";
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(countB.Text) != 30)
            {
                count++;
                countB.Text = count.ToString();
            }
            else
            {
                count = 30;
                countB.Text = count.ToString();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(countB.Text) != 5)
            {
                count--;
                countB.Text = count.ToString();
            }
            else
            {
                count = 5;
                countB.Text = count.ToString();
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Start.IsEnabled = true;
            Stop.IsEnabled = false;
        }
    }
}
