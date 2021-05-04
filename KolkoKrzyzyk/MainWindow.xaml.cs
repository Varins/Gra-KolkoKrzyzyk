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
using ServiceKolkoKrzyzyk;

namespace KolkoKrzyzyk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();      
        }

        private void GridButton_Click(object sender, RoutedEventArgs e)
        {
            GraKolkoKrzyzykClient client = new GraKolkoKrzyzykClient();
            Button btn = (Button)sender;
            int row = 3, col = 3;
            switch (btn.Name)
            {
                case ("Btn0"):
                    {
                        col = 0;
                        row = 0;
                        break;
                    }
                case ("Btn1"):
                    {
                        col = 1;
                        row = 0;
                        break;
                    }
                case ("Btn2"):
                    {
                        col = 2;
                        row = 0;
                        break;
                    }
                case ("Btn3"):
                    {
                        col = 0;
                        row = 1;
                        break;
                    }
                case ("Btn4"):
                    {
                        col = 1;
                        row = 1;
                        break;
                    }
                case ("Btn5"):
                    {
                        col = 2;
                        row = 1;
                        break;
                    }
                case ("Btn6"):
                    {
                        col = 0;
                        row = 2;
                        break;
                    }
                case ("Btn7"):
                    {
                        col = 1;
                        row = 2;
                        break;
                    }
                case ("Btn8"):
                    {
                        col = 2;
                        row = 2;
                        break;
                    }
            }
            if(col < 3 && row < 3)
            {
                int outRow=3, outCol=3;
                if(client.WykonajRuch(row, col, out outRow, out outRow))
                {
                    client.SprawdzWygrana(row, col);
                    if(outRow + outCol < 5)
                        client.SprawdzWygrana(outRow, outCol);
                }
            }
        }
    }
}
