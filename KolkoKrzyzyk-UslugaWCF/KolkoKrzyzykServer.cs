using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace KolkoKrzyzyk_UslugaWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class KolkoKrzyzykServer : IGraKolkoKrzyzyk
    {
        private const int W = 3, K = 3;

        private static int[,] ArrayPlansza = new int[W, K];

        private static List<int> ListaRuchow = new List<int>();

        private const int ZnakKolko = 1, ZnakKrzyzyk = 2;

        private Random rnd = new Random();

        public int SprawdzWygrana(int wiersz, int kolumna)
        {
            int suma = wiersz + kolumna;
            // 0 1 2
            // 1 2 3
            // 2 3 4
            // powyżej rozrysowana jest tabelka, w której podano sumy poszczególnych kombinacji indeksów, można zauważyć,
            // że dla sum nieparzystych nie musimy sprawdzac ukosów, bo nie zawierają się w nich
            // idąc dalej, dla sumy = 0 lub = 4 sprawdzamy ukos "\"
            // dla sumy = 2 (gdzie wiersz i kolumna są parzyste) sprawdzamy ukos "/"
            // dla sumy = 2 (gdzie przynajmniej wiersz lub kolumna jest nieparzysta) spradzamy oba ukosy
            if(suma % 2 == 0) 
            {
                if(suma == 2)
                {
                    //jesli indeksy są 1, 1, to sprawdzamy ukos "\"
                    if(wiersz == 1 && kolumna == 1)
                        if((ArrayPlansza[0,0] + ArrayPlansza[1, 1] + ArrayPlansza[2, 2]) % 3 == 0)
                            return 4;
                    //Sprawdzanie ukosu "/"
                    if ((ArrayPlansza[0, 2] + ArrayPlansza[1, 1] + ArrayPlansza[2, 0]) % 3 == 0)
                        return 8;

                }//Sprawdzanie ukosu "\"
                else
                    if ((ArrayPlansza[0, 0] + ArrayPlansza[1, 1] + ArrayPlansza[2, 2]) % 3 == 0)
                        return 4;
            }
            //sprawdzamy czy suma wiersza = 3 lub 6, tzn czy na każdym miejscu jest 1 lub na każdym 2
            if ((ArrayPlansza[wiersz, 0] + ArrayPlansza[wiersz, 1] + ArrayPlansza[wiersz, 2]) % 3 == 0) 
                return 2;
            //sprawdzamy czy suma kolumny = 3 lub 6, tzn czy na każdym miejscu jest 1 lub na każdym 2
            if ((ArrayPlansza[0, kolumna] + ArrayPlansza[1, kolumna] + ArrayPlansza[2, kolumna]) % 3 == 0)
                return 1;
            //Brak zwycięstw
            return 0;
        }

        public void Start()
        {
            //Nie trzeba wypełniać ArrayPlansza zerami, bo new int[W,K] wywołuje domyślny konstruktor dla każdego pola, a domyślny konstruktor int ustawia wartość na 0
            for(int i = 0; i<9; i++)
            {
                ListaRuchow.Add(i);
            }

        }

        public bool WykonajRuch(int wiersz, int kolumna, out int wierszServer, out int kolumnaServer)
        {
            if(ArrayPlansza[wiersz, kolumna] == 0)
            {
                ArrayPlansza[wiersz, kolumna] = 1;
                ListaRuchow.Remove(wiersz * K + kolumna);
                int indeks = ListaRuchow[rnd.Next(0, ListaRuchow.Count - 1)];
                kolumnaServer = indeks % K;
                wierszServer = (indeks - kolumnaServer) / K;
                ArrayPlansza[wierszServer, kolumnaServer] = 2;
                ListaRuchow.Remove(indeks);
                return true;
            }
            else
            {
                wierszServer = 9;
                kolumnaServer = 9;
                return false;
            }
        }



    }

}
