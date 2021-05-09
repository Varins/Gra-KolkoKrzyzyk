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
        GraKolkoKrzyzykClient client;
        private List<Button> gridBtns;

        public MainWindow()
        {
            InitializeComponent();
            client = new GraKolkoKrzyzykClient();
            gridBtns = new List<Button>();
            lbl_info.Content = "";
            gridBtns.Add(Btn0);
            gridBtns.Add(Btn1);
            gridBtns.Add(Btn2);
            gridBtns.Add(Btn3);
            gridBtns.Add(Btn4);
            gridBtns.Add(Btn5);
            gridBtns.Add(Btn6);
            gridBtns.Add(Btn7);
            gridBtns.Add(Btn8);
            foreach (Button btn in gridBtns)
            {
                btn.Content = "";
                btn.IsEnabled = false;
            }
            Btn_start.IsEnabled = true;
            Btn_reset.IsEnabled = false;
        }

        private string decodeWin(int win)
        {
            string info = "linię";
            switch (win)
            {
                case 1:
                    {
                        info = "linię pionową";
                        break;
                    }
                case 2:
                    {
                        info = "linię poziomą";
                        break;
                    }
                case 4:
                    {
                        info = "linię ukośną od lewej do prawej";
                        break;
                    }
                case 8:
                    {
                        info = "linię ukośną od prawej do lewej";
                        break;
                    }
            }
            return info;
        }

        private void GridButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int row = 3, col = 3;
            int index = gridBtns.IndexOf(btn);
            col = index % 3;
            row = (index - col) / 3;
            if (col < 3 && row < 3)
            {
                if (client.WykonajRuch(row, col, (bool)radioO.IsChecked, out int outRow, out int outCol))
                {
                    gridBtns[index].Content = (bool)radioO.IsChecked ? "O" : "X";
                    gridBtns[index].IsEnabled = false;
                    int wygrana = client.SprawdzWygrana(row, col);
                    if ( wygrana != 0)
                    {
                        
                        lbl_info.Content = "Wygrałeś tworząc " + decodeWin(wygrana) + "!";
                        foreach (Button bttn in gridBtns)
                        {
                            bttn.IsEnabled = false;
                            Btn_start.IsEnabled = true;
                        }
                    }
                    else
                    {
                        if(outRow + outCol < 5)
                        {
                            index = outRow * 3 + outCol;
                            gridBtns[index].Content = (bool)radioX.IsChecked ? "O" : "X";
                            gridBtns[index].IsEnabled = false;
                            wygrana = client.SprawdzWygrana(outRow, outCol);
                            if (wygrana != 0)
                            {
                                lbl_info.Content = "Wygrał serwer tworząc " + decodeWin(wygrana) + "!";
                                foreach (Button bttn in gridBtns)
                                {
                                    bttn.IsEnabled = false;
                                }
                                Btn_start.IsEnabled = true;
                                radioO.IsEnabled = true;
                                radioX.IsEnabled = true;
                            }
                            else
                            {
                                lbl_info.Content = "Jeszcze nie wygrałeś, wykonaj następny ruch";
                            }
                        }
                        else
                        {
                            lbl_info.Content = "Remis";
                            foreach (Button bttn in gridBtns)
                            {
                                bttn.IsEnabled = false;
                            }
                            Btn_start.IsEnabled = true;
                            radioO.IsEnabled = true;
                            radioX.IsEnabled = true;
                        }
                    }
                }
                else
                {
                    lbl_info.Content = "Nie można wykonać ruchu, wybierz inne pole.";
                }
            }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            client.Start();
            foreach (Button btn in gridBtns)
            {
                btn.Content = "";
                btn.IsEnabled = true;
            }
            Btn_start.IsEnabled = false;
            Btn_reset.IsEnabled = true;
            radioO.IsEnabled = false;
            radioX.IsEnabled = false;
            lbl_info.Content = "Wykonaj pierwszy ruch";
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in gridBtns)
            {
                btn.Content = "";
                btn.IsEnabled = false;
            }
            Btn_start.IsEnabled = true;
            Btn_reset.IsEnabled = false;
            radioO.IsEnabled = true;
            radioX.IsEnabled = true;
            lbl_info.Content = "Zresetowano planszę";
        }
    }
}
