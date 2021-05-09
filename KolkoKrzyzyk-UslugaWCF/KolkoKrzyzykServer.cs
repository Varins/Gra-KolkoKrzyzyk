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

        private static int[,] ArrayPlansza;

        private static List<int> ListaRuchow;

        private const int ZnakKolko = 1, ZnakKrzyzyk = 2;

        private Random rnd = new Random();
        /// <summary>
        /// Funkcja sprawdza, czy dla podanej lokalizacji ostatniego zaznaczenia istnieje zwycięskie ułożenie
        /// </summary>
        /// <param name="wiersz"></param>
        /// <param name="kolumna"></param>
        /// <returns> 0 - brak wygranej, 1 - linia w pionie, 2 - linia w poziomie, 4 - linia ukośna od lewej do prawej, 8 - linia ukośna od prawej do lewej</returns>
        public int SprawdzWygrana(int wiersz, int kolumna)
        {
            int suma = wiersz + kolumna;
            int znak = ArrayPlansza[wiersz, kolumna];
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
                        if(ArrayPlansza[0,0] == znak && ArrayPlansza[1, 1] == znak && ArrayPlansza[2, 2] == znak)
                            return 4;
                    //Sprawdzanie ukosu "/"
                    if (ArrayPlansza[0, 2] == znak && ArrayPlansza[1, 1] == znak && ArrayPlansza[2, 0] == znak)
                        return 8;

                }//Sprawdzanie ukosu "\"
                else
                    if (ArrayPlansza[0, 0] == znak && ArrayPlansza[1, 1] == znak && ArrayPlansza[2, 2] == znak)
                        return 4;
            }
            //sprawdzamy czy suma wiersza = 3 lub 6, tzn czy na każdym miejscu jest 1 lub na każdym 2
            if (ArrayPlansza[wiersz, 0] == znak && ArrayPlansza[wiersz, 1] == znak && ArrayPlansza[wiersz, 2] == znak) 
                return 2;
            //sprawdzamy czy suma kolumny = 3 lub 6, tzn czy na każdym miejscu jest 1 lub na każdym 2
            if (ArrayPlansza[0, kolumna] == znak && ArrayPlansza[1, kolumna] == znak && ArrayPlansza[2, kolumna] == znak)
                return 1;
            //Brak zwycięstw
            return 0;
        }

        public void Start()
        {
            ArrayPlansza = new int[W, K];
            ListaRuchow = new List<int>();
            for (int i = 0; i<9; i++)
            {
                ListaRuchow.Add(i);
            }
        }

        public bool WykonajRuch(int wiersz, int kolumna, bool znak, out int wierszServer, out int kolumnaServer)
        {
            wierszServer = 3;
            kolumnaServer = 3;
            if (ArrayPlansza[wiersz, kolumna] == 0)
            {
                ArrayPlansza[wiersz, kolumna] = znak ? ZnakKolko : ZnakKrzyzyk;
                ListaRuchow.Remove(wiersz * K + kolumna);
                if(ListaRuchow.Count>0)
                {
                    int indeks = ListaRuchow[rnd.Next(0, ListaRuchow.Count - 1)];
                    kolumnaServer = indeks % K;
                    wierszServer = (indeks - kolumnaServer) / K;
                    ArrayPlansza[wierszServer, kolumnaServer] = znak ? ZnakKrzyzyk : ZnakKolko;
                    ListaRuchow.Remove(indeks);
                }
                return true;
            }
            else
            {
                return false;
            }
        }



    }

}
