﻿using System;

namespace AlgorytmyDoTTP.Struktura.Moduly.Algorytmy.Ewolucyjny
{
    class cSEA : iSEA
    {
        static Random random = new Random();

        // arg x maxymilizujący funkcję: f(x) = xsin(x) * sin(10*x), x od -1 do 4, [-1, 4], dokładność rozwiązania do 0.001
        public void Start()
        {
            int iteracje = 100;
            double pwoMutacji = 0.001;
            double pwoKrzyzowania = 0.5;
            ushort rozmiarPopulacji = 5000;

            ushort[] populacja = this.StworzPopulacje(rozmiarPopulacji);


            while (iteracje > 0)
            {
                ushort[] nowaPopulacja = new ushort[rozmiarPopulacji];

                for (int i = 0; i < rozmiarPopulacji; i++)
                {
                    ushort mama = this.Turniej(populacja),
                       tata = this.Turniej(populacja),
                       dziecko = this.Krzyzowanie(mama, tata);

                    nowaPopulacja[i] = dziecko;
                }

                populacja = nowaPopulacja;
                --iteracje;
            }

            for (int i = 0; i < populacja.Length; i++)
            {
                Console.WriteLine("dla x=" + this.Fenotyp(populacja[i]) + " f(x)=" + this.FunkcjaDopasowania(this.Fenotyp(populacja[i])));
            }

            Console.ReadLine();
        }

        public ushort Mutacja(ushort geny)
        {
            if (random.NextDouble() > 0.1)
            {
                return geny;
            }

            int bit = random.Next(12);
            ushort maska = (ushort)(1 << bit);

            return (ushort)(maska ^ geny);
        }

        public ushort Krzyzowanie(ushort mama, ushort tata)
        {
            int ciecie = random.Next(1, 12 + 1);
            ushort maska = 0;

            maska = (ushort)~maska;
            maska <<= ciecie;

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("maska="+maska);
            Console.WriteLine("mama=" + mama);
            Console.WriteLine("tata=" + tata);

            ushort dzieciak = (ushort)(maska & mama);
            Console.WriteLine("dzieciak przed=" + dzieciak);
            dzieciak |= (ushort)(~maska & tata);
            Console.WriteLine("dzieciak po=" + dzieciak);

            return this.Mutacja(dzieciak);
        }

        public double Fenotyp(ushort genotyp)
        {
            return -1 + genotyp / 1000;
        }

        public double FunkcjaDopasowania(double x)
        {
            return x * Math.Sin(x) * Math.Sin(10 * x);
        }

        public ushort[] StworzPopulacje(ushort rozmiarPopulacji)
        {
            ushort[] populacja = new ushort[rozmiarPopulacji];

            for (int i = 0; i < rozmiarPopulacji; i++)
            {
                populacja[i] = (ushort)random.Next(5000 + 1);
            }

            return populacja;
        }

        public ushort Turniej(ushort[] populacja)
        {
            int k1 = random.Next(populacja.Length - 1),
                k2 = random.Next(populacja.Length - 1);

            if (this.FunkcjaDopasowania(this.Fenotyp(populacja[k1])) > this.FunkcjaDopasowania(this.Fenotyp(populacja[k2])))
            {
                return populacja[k1];
            }
            else
            {
                return populacja[k2];
            }
        }
    }
}
