﻿using BiPA.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using BiPA.Struktura.ProblemyOptymalizacyjne.KP;
using BiPA.Struktura.ProblemyOptymalizacyjne.TSP;
using BiPA.Struktura.ProblemyOptymalizacyjne.TTP.Model;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BiPA.Struktura.ProblemyOptymalizacyjne.TTP
{
    /// <summary>
    /// Klasa konkretna reprezentująca Problem Podróżującego Złodzieja
    /// </summary>
    class ProblemPodrozujacegoZlodzieja : ProblemOptymalizacyjny
    {
        private string modelTTP;
        private ushort[][] dostepnePrzedmioty;
        private ProblemPlecakowy problemPlecakowy;
        private ProblemKomiwojazera problemKomiwojazera;

        public ProblemPodrozujacegoZlodzieja(string nazwaPakietu, string modelTTP)
        {
            this.modelTTP = modelTTP;
            Inicjalizacja(nazwaPakietu);
        }

        /// <summary>
        /// Metoda konwertuje dane w pliku XML pod struktury dla C#
        /// </summary>
        /// <param name="nazwaPakietu">Nazwa pliku xml zawierającego dane</param>
        private void Inicjalizacja(string nazwaPakietu)
        {
            // pobranie pliku z danymi z dysku
            XmlDocument dokument = new XmlDocument();
            dokument.Load("./Dane/TTP/" + nazwaPakietu + ".xml");

            // pobranie danych składowych problemów optymalizacyjnych z dysku
            XmlNode przypadekTSP = dokument.DocumentElement.SelectSingleNode("/korzen/tsp");
            XmlNode przypadekKP = dokument.DocumentElement.SelectSingleNode("/korzen/kp");

            // utworzenie instancji składowych problemów optymalizacyjnych
            problemPlecakowy = new ProblemPlecakowy(przypadekKP.InnerText);
            problemKomiwojazera = new ProblemKomiwojazera(przypadekTSP.InnerText);

            // pobranie inforamcji o dostępnościach przedmiotów w poszczególnych miastach
            XmlNodeList rozmieszczeniePrzedmiotow = dokument.DocumentElement.SelectNodes("/korzen/dostepnePrzedmioty/miasto");
            dlugoscGenotypu = (ushort)rozmieszczeniePrzedmiotow.Count;

            dostepnePrzedmioty = new ushort[problemKomiwojazera.ZwrocDlugoscGenotypu()][];
            for(int i = 0; i < problemKomiwojazera.ZwrocDlugoscGenotypu(); i++)
            {
                // dla każdego i-tego miasta przypisanie dostępności przedmiotów
                dostepnePrzedmioty[i] = new ushort[problemPlecakowy.ZwrocDlugoscGenotypu()];
                dostepnePrzedmioty[i] = (ushort[])((new Miasto()).ZwrocPrzedmioty(
                    rozmieszczeniePrzedmiotow[i].InnerText, 
                    problemPlecakowy.ZwrocDlugoscGenotypu()
                    ).Clone());
            }
        }

        public override ushort[][] ZwrocDostepnePrzedmioty()
        {
            return dostepnePrzedmioty;
        }

        public ProblemPlecakowy ZwrocProblemPlecakowy()
        {
            return problemPlecakowy;
        }

        public ProblemKomiwojazera ZwrocProblemKomiwojazera()
        {
            return problemKomiwojazera;
        }

        public override Dictionary<string, float[]> ObliczZysk(Dictionary<string, ushort[][]> macierz)
        {
            ITTP obiektTTP = new TTP1();
            // pobranie planu podróży przez miasta
            IElement[] planPodrozy = problemKomiwojazera.ZwrocWybraneElementy(macierz["tsp"][0]);
            // pobranie długości trasy jako wektora pomiędzy wybranymi miastami
            float[] dlugosciTrasy = problemKomiwojazera.ZwrocDlugoscTrasy(planPodrozy, true);

            if (modelTTP == "TTP2") obiektTTP = new TTP2();

            return obiektTTP.ObliczWartoscFunkcjiCelu(dlugosciTrasy, macierz["kp"], ZwrocOgraniczeniaProblemu(), problemPlecakowy);
        }

        public override Dictionary<string, ushort[][]> ZwrocWybraneElementy(ushort[][] wybraneElementy)
        {
            ushort[][] tsp = new ushort[1][],
                       kp = new ushort[wybraneElementy.Length][];

            // TSP będzie reprezentowany jako wektor miast
            tsp[0] = new ushort[wybraneElementy.Length];

            for(int i = 0; i < wybraneElementy.Length; i++)
            {
                // pierwszy element i-tego wiersza macierzy, jest miastem
                tsp[0][i] = wybraneElementy[i][0];
                // reszta elementów w i-tym wierszu to wartości ze zbioru {0,1}
                // odpowiadające czy zebrano dany przedmiot z i-tego miasta
                kp[i] = new ushort[wybraneElementy[0].Length - 1];

                for(int j = 1; j < wybraneElementy[0].Length; j++)
                {
                    // dopisanie wartości ze zbioru {0,1} do i-tego miasta
                    kp[i][j - 1] = wybraneElementy[i][j];
                }
            }

            Dictionary<string, ushort[][]> wynik = new Dictionary<string, ushort[][]>();

            wynik["tsp"] = tsp;
            wynik["kp"] = kp;

            return wynik;
        }

        public override IElement[] ZwrocWybraneElementy(ushort[] wybraneElementy)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, float[]> ObliczZysk(IElement[] wektor)
        {
            throw new NotImplementedException();
        }
    }
}
