﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.KP;
using System.Collections;

namespace AlgorytmyDoTTP.Struktura.Algorytmy.Ewolucyjny
{
    class Osobnik
    {
        private ProblemPlecakowy problemPlecakowy;

        public Osobnik(ProblemPlecakowy problemPlecakowy)
        {
            this.problemPlecakowy = problemPlecakowy;
        }

        public ArrayList Fenotyp(ushort[] genotyp)
        {
            return problemPlecakowy.ZwrocWybraneElementy(genotyp);
        }

        public double[] FunkcjaDopasowania(ArrayList przedmioty)
        {
            return problemPlecakowy.ObliczZysk(przedmioty);
        }
    }
}
