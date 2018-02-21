﻿using System;

namespace AlgorytmyDoTTP
{
    class Przedstawiciel
    {
        private int fenotyp;
        private int pokolenie;
        private int[] chromosom;
        private double przydatnosc;

        public Przedstawiciel(int[] chromosom, int fenotyp, double przydatnosc, int pokolenie)
        {
            this.chromosom = chromosom;
            this.fenotyp = fenotyp;
            this.przydatnosc = przydatnosc;
            this.pokolenie = pokolenie;
        }

        public int[] zwrocChromosom()
        {
            return chromosom;
        }

        public int zwrocFenotyp()
        {
            return fenotyp;
        }

        public double zwrocPrzydatnosc()
        {
            return przydatnosc;
        }

        public int zwrocPokolenie()
        {
            return pokolenie;
        }

        override
        public String ToString()
        {
            String chromosomStr = string.Join("", chromosom);

            return "Chromosom: "+ chromosomStr +
                    "\nfenotyp: "+ fenotyp +
                    "\nprzydatnosc: " + przydatnosc +
                    "\npokolenie: "+ pokolenie;
        }
    }
}
