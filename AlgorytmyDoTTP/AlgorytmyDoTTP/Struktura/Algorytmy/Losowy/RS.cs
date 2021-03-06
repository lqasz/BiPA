﻿using BiPA.Struktura.Algorytmy.Abstrakcyjny;
using BiPA.Struktura.Algorytmy.Abstrakcyjny.Analityka;
using BiPA.Struktura.Algorytmy.Losowy.Losowanie;
using BiPA.Widoki.Narzedzia;
using System;
using System.Threading.Tasks;

namespace BiPA.Struktura.Algorytmy.Losowy
{
    /// <summary>
    /// Klasa konkretna Algorytmu Losowego.
    /// Pozwala na wyszukanie rozwiązania optymalnego danego problemu wg zasad działania Algorytmów Losowego
    /// </summary>
    class RS : IAlgorytm
    {
        private readonly int iloscRozwiazan;
        private readonly int iloscElementow;
        private ALosowanie losowanie;
        private AnalizaRLS_RS analityka;

        public RS()
        {
            throw new Exception(); // błąd, nie zbudowano kontekstu pod wybrany problem optymalizacyjny
        }

        public RS(ALosowanie losowanie, int iloscRozwiazan, int iloscElementow, AnalizaRLS_RS analityka)
        {
            this.losowanie = losowanie;
            this.iloscRozwiazan = iloscRozwiazan;
            this.iloscElementow = iloscElementow;
            this.analityka = analityka;
        }

        public Task Start(IProgress<PostepBadania> postep)
        {
            int czas = 0,
                poprzedniaSekunda = -1,
                calkowityCzas = analityka.ZwrocLiczbeIteracji() * analityka.ZwrocCzasDzialaniaAlgorytmu();
            PostepBadania postepBadania = new PostepBadania();

            return Task.Run(() =>
            {
                for (short i = 0; i < analityka.ZwrocLiczbeIteracji(); i++)
                {
                    analityka.RozpocznijPomiarCzasu(); // rozpoczęcie pomiaru czasu

                    while (analityka.IleCzasuDzialaAlgorytm("s") < analityka.ZwrocCzasDzialaniaAlgorytmu())
                    {
                        losowanie.SzukajNajlepszegoRozwiazania(iloscRozwiazan, iloscElementow);
                        analityka.DopiszWartoscProcesu(i, (int)analityka.IleCzasuDzialaAlgorytm("s"), losowanie.ZwrocNajlepszeRozwiazanie());

                        if (poprzedniaSekunda == -1 || poprzedniaSekunda != (int)analityka.IleCzasuDzialaAlgorytm("s"))
                        {
                            czas++;
                            poprzedniaSekunda = (int)analityka.IleCzasuDzialaAlgorytm("s");
                        }

                        postepBadania.ProcentUkonczenia = (czas * 100 / calkowityCzas) - 1;
                        if (postepBadania.ProcentUkonczenia < 0) postepBadania.ProcentUkonczenia = 0;
                        if (postepBadania.ProcentUkonczenia > 100) postepBadania.ProcentUkonczenia = 100;
                        postep.Report(postepBadania);
                    }

                    analityka.ResetPomiaruCzasu(); // zakończenie pomiaru czasu
                    poprzedniaSekunda = -1;
                }
            
                analityka.ObliczSrednieWartosciProcesu();
            });
        }

        public AAnalityka ZwrocAnalityke()
        {
            return analityka;
        }
    }
}
