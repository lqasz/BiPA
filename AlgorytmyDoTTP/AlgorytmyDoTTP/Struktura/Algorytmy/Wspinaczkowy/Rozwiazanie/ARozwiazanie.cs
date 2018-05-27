﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Wspinaczkowy.Rozwiazanie
{
    abstract class ARozwiazanie
    {
        protected Random losowy = new Random();

        public abstract ushort[] ZwrocRozwiazanie();

        public abstract void UstawRozwiazanie(ushort[] rozwiazanie);

        public abstract Dictionary<string, double[]> ZnajdzOptimum();
    }
}