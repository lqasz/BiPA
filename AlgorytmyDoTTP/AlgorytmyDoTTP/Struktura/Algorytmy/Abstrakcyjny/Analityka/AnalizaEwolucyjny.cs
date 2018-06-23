﻿using AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny.Osobnik;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Abstrakcyjny.Analityka
{
    /// <summary>
    /// Klasa analityczna.
    /// Rozszerzenie podstawowej klasy analitycznej, dla Algorytmu Ewolucyjnego.
    /// </summary>
    class AnalizaEwolucyjny : AnalizaRLS_RS
    {
        private AOsobnik rozwiazanie;
        private double najlepszaWartoscFunkcji;
        private ReprezentacjaRozwiazania najlepszeRozwiazanie;

        public AnalizaEwolucyjny(AOsobnik rozwiazanie)
        {
            this.rozwiazanie = rozwiazanie;
            najlepszaWartoscFunkcji = -10000;
            najlepszeRozwiazanie = new ReprezentacjaRozwiazania();
        }

        //public double DominantaPopulacji(ArrayList populacja)
        //{
        //    double wynik = rozwiazanie.FunkcjaDopasowania((ReprezentacjaRozwiazania)populacja[0])["max"][0];

        //    Dictionary<double, int> wartosci = new Dictionary<double, int>();

        //    for (int i = 0; i < populacja.Count; i++)
        //    {
        //        wartosci[rozwiazanie.FunkcjaDopasowania((ReprezentacjaRozwiazania)populacja[i])["max"][0]] =
        //    }

        //    return wynik;
        //}

        /// <summary>
        /// Metoda wyznaczająca medianę funkcji celu z populacji rozwiązań
        /// </summary>
        /// <param name="populacja">Lista rozwiązań</param>
        /// <returns>Zwraca medianę</returns>
        public double MedianaPopulacji(ArrayList populacja)
        {
            int srodek = populacja.Count / 2;
            double[] wynikiPopulacji = new double[populacja.Count]; // tablica zawierająca wartości funkcji celu z listy rozwiązań

            for(int i = 0; i < populacja.Count; i++)
            {
                wynikiPopulacji[i] = rozwiazanie.FunkcjaDopasowania((ReprezentacjaRozwiazania)populacja[i])["max"][0];
            }

            // sortowanie wyników rosnąco
            Array.Sort(wynikiPopulacji);

            return wynikiPopulacji[srodek];
        }

        /// <summary>
        /// Metoda obliczająca średnią wartość funkcji celu z populacji rozwiązań
        /// </summary>
        /// <param name="populacja">Lista rozwiązań</param>
        /// <returns>Zwraca wartość średnią</returns>
        public double SredniaPopulacji(ArrayList populacja)
        {
            double wynik = 0;

            foreach (ReprezentacjaRozwiazania osobnik in populacja)
            {
                wynik += rozwiazanie.FunkcjaDopasowania(osobnik)["max"][0];
            }

            return wynik / populacja.Count;
        }

        /// <summary>
        /// Metoda obliczająca odchylenie standardowe funkcji celu z populacji rozwiązań
        /// </summary>
        /// <returns>Zwraca odchylenie standardowe</returns>
        /// <param name="populacja">Lista rozwiązań</param>
        /// <param name="srednia">Średnia z listy rozwiązań</param>
        public double OdchylenieStandardowePopulacji(ArrayList populacja, double srednia)
        {
            double sumaKwadratow = 0;

            foreach (ReprezentacjaRozwiazania osobnik in populacja)
            {
                sumaKwadratow += Math.Pow(rozwiazanie.FunkcjaDopasowania(osobnik)["max"][0], 2);
            }

            double sredniaSumaKwadratow = sumaKwadratow / (populacja.Count - 1);
            return Math.Sqrt(sredniaSumaKwadratow - (Math.Pow(srednia, 2)));
        }

        /// <summary>
        /// Metoda poszukująca najlepszego rozwiązania znalezionego do tej pory
        /// </summary>
        /// <param name="geny">Tablica definiująca dziedzinę rozwiązania</param>
        public void ZmienWartoscNiebo(ReprezentacjaRozwiazania genotyp)
        {
            if(najlepszaWartoscFunkcji < rozwiazanie.FunkcjaDopasowania(genotyp)["max"][0])
            {
                najlepszeRozwiazanie = genotyp;
                najlepszaWartoscFunkcji = rozwiazanie.FunkcjaDopasowania(genotyp)["max"][0];
            }
        }

        /// <summary>
        /// Metoda zwracająca najlepszą dziedzinę rozwiązania
        /// </summary>
        /// <returns>Dziedzina rozwiązania</returns>
        public ReprezentacjaRozwiazania ZwrocNajlepszyGenotyp()
        {
            return najlepszeRozwiazanie;
        }

        public string ZwrocNajlepszeRowziazanie()
        {
            return DekodujRozwiazanie(najlepszeRozwiazanie);
        }

        /// <summary>
        /// Metoda zwracająca najlepszą znalezioną wartość
        /// </summary>
        /// <returns>Wartość funkcji celu</returns>
        public Dictionary<string, double[]> ZwrocWartoscNiebo()
        {
            return rozwiazanie.FunkcjaDopasowania(najlepszeRozwiazanie);
        }
    }
}
