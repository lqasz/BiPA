﻿using System;

namespace AlgorytmyDoTTP
{
    class AlgorytmEwolucyjny
    {
        private double pwoMutacji;
        private double pwoKrzyzowania;
        private double najlepszyWynik = 0;
        private double[] srednia;

        public AlgorytmEwolucyjny(double pwoKrzyzowania, double pwoMutacji)
        {
            this.pwoKrzyzowania = pwoKrzyzowania;
            this.pwoMutacji = pwoMutacji;
        }

        public void wykonajEwolucje(int wielkoscPopulacji, int dlugoscChromosomu, int ileRazy)
        {
            this.srednia = new double[ileRazy];
            OperacjeNaPopulacji operacje = new OperacjeNaPopulacji(pwoKrzyzowania, pwoMutacji);
            Populacja populacjaBazowa = new Populacja(wielkoscPopulacji, dlugoscChromosomu);

            for (int i = 0; i < ileRazy; i++)
            {
                this.srednia[i] = 0;
                Przedstawiciel[] przedstawiciele = populacjaBazowa.zwrocPopulacje();
                wielkoscPopulacji = (przedstawiciele.Length % 2 == 1) ? przedstawiciele.Length - 1 : przedstawiciele.Length;

                int iter = 0;
                int[][] chromosomy = new int[wielkoscPopulacji][];
                for (int j = 1; j <= wielkoscPopulacji; j += 2)
                {
                    if (wielkoscPopulacji <= 2) break;

                    if (przedstawiciele[j].zwrocPrzydatnosc() > this.najlepszyWynik)
                    {
                        this.najlepszyWynik = przedstawiciele[j].zwrocPrzydatnosc();
                        this.srednia[i] += (przedstawiciele[j].zwrocPrzydatnosc()) / 2;
                    }

                    if (przedstawiciele[j - 1].zwrocPrzydatnosc() > this.najlepszyWynik)
                    {
                        this.najlepszyWynik = przedstawiciele[j-1].zwrocPrzydatnosc();
                        this.srednia[i] += (przedstawiciele[j-1].zwrocPrzydatnosc()) / 2;
                    }

                    int[][] nastepnaGeneracja = operacje.krzyzowanie(przedstawiciele[j].zwrocChromosom(), przedstawiciele[j - 1].zwrocChromosom(), dlugoscChromosomu);

                    chromosomy[iter] = nastepnaGeneracja[0];
                    iter++;     
                    chromosomy[iter] = nastepnaGeneracja[1];
                    iter++;
                }

                wielkoscPopulacji = iter;

                populacjaBazowa.nowaPopulacja(chromosomy, wielkoscPopulacji, i);
                przedstawiciele = operacje.wybierzOsobniki(populacjaBazowa.zwrocPopulacje());
            }
        }

        public double zwrocNajlepszyWynik()
        {
            return this.najlepszyWynik;
        }

        public String zwrocSrednia()
        {
            int i = 1;
            String wynik = "";

            foreach (double sredniaGeneracji in this.srednia)
            {
                wynik += " Generacja: " + i + " | średnia: " + sredniaGeneracji;
                i++;
            }

            return wynik;
        }
    }
}
