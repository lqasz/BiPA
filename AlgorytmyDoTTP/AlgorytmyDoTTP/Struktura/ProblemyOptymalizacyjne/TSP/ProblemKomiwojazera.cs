﻿using BiPA.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BiPA.Struktura.ProblemyOptymalizacyjne.TSP
{
    /// <summary>
    /// Klasa konkretna reprezentująca Problem Komiwojażera
    /// </summary>
    class ProblemKomiwojazera : ProblemOptymalizacyjny
    {
        public ProblemKomiwojazera(string nazwaPakietu)
        {
            Inicjalizacja(nazwaPakietu);
        }

        /// <summary>
        /// Metoda konwertuje dane w pliku XML pod struktury dla C#
        /// </summary>
        /// <param name="nazwaPakietu">Nazwa pliku xml zawierającego dane</param>
        private void Inicjalizacja(string nazwaPakietu)
        {
            XmlDocument dokument = new XmlDocument();
            dokument.Load("./Dane/TSP/" + nazwaPakietu + ".xml");
            XmlNodeList miasta = dokument.DocumentElement.SelectNodes("/korzen/mapa/miasto");
            
            dlugoscGenotypu = (ushort)(miasta.Count);

            int liczbaPermutacji = 0;
            for(ushort i = (ushort)(dlugoscGenotypu - 1); i > 0; i--)
            {
                liczbaPermutacji += i;
            }
            
            instancje = new Miasto[liczbaPermutacji];

            int iter = 0;
            for (ushort i = 0; i < miasta.Count; i++)
            {
                for (ushort j = 0; j < miasta.Count; j++)
                {
                    if (i < j)
                    {
                        float x1 = float.Parse(miasta[i]["x"].InnerText),
                              y1 = float.Parse(miasta[i]["y"].InnerText),
                              x2 = float.Parse(miasta[j]["x"].InnerText),
                              y2 = float.Parse(miasta[j]["y"].InnerText),
                              dystans = (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                        
                        instancje[iter] = new Miasto((ushort)(i + 1), (ushort)(j + 1), dystans);
                        iter++;
                    }
                }
            }
        }

        public override Dictionary<string, float[]> ObliczZysk(IElement[] wektor)
        {
            Dictionary<string, float[]> wynik = new Dictionary<string, float[]>();
            float zysk = ZwrocDlugoscTrasy(wektor, false)[0];

            wynik["min"] = new float[] { zysk };
            wynik["max"] = new float[] { zysk * -1 };

            return wynik;
        }

        public float[] ZwrocDlugoscTrasy(IElement[] wektor, bool zwrocWektor)
        {
            int dlugoscWekotra = wektor.Length;
            float[] wynik = (zwrocWektor) ? new float[dlugoscWekotra] : new float[] { 0 };

            for (int i = 0; i < dlugoscWekotra; i++)
            {
                if (zwrocWektor)
                {
                    wynik[i] = wektor[i].ZwrocDlugosc();
                }
                else
                {
                    wynik[0] += wektor[i].ZwrocDlugosc();
                }
            }

            return wynik;
        }

        public override IElement[] ZwrocWybraneElementy(ushort[] elementy)
        {
            int liczbaMiast = elementy.Length - 1;
            IElement[] wybraneElementy = new IElement[liczbaMiast];

            for (int i = 0; i < liczbaMiast; i++)
            {
                int j = (i == liczbaMiast - 1) ? 0 : i + 1;

                for (int k = 0; k < instancje.Length; k++)
                {
                    int start = (elementy[j] < elementy[i]) ? elementy[j] : elementy[i],
                        koniec = (elementy[i] < elementy[j]) ? elementy[j] : elementy[i];

                    if (start == instancje[k].ZwrocOd() && koniec == instancje[k].ZwrocDo())
                    {
                        wybraneElementy[i] = instancje[k];
                        break;
                    }
                }
            }

            return wybraneElementy;
        }

        public override Dictionary<string, ushort[][]> ZwrocWybraneElementy(ushort[][] wybraneElementy)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, float[]> ObliczZysk(Dictionary<string, ushort[][]> wektor)
        {
            throw new NotImplementedException();
        }

        public override ushort[][] ZwrocDostepnePrzedmioty()
        {
            throw new NotImplementedException();
        }
    }
}