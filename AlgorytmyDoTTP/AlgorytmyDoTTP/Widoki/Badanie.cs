﻿using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.Abstrakcyjny;
using AlgorytmyDoTTP.Struktura.ProblemyOptymalizacyjne.KP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgorytmyDoTTP.Widoki
{
    public partial class Badanie : Form
    {
        private Dictionary<string, string> parametry;

        public Badanie(Dictionary<string, string> parametry)
        {
            InitializeComponent();
            this.parametry = parametry;
        }
        
        private void Badanie_Load(object sender, EventArgs e)
        {
            ProblemOptymalizacyjny problem = new ProblemPlecakowy(parametry["dane"]);
            problem.UstawOgraniczeniaProblemu(7);

            wynikiBadania.Text = (new Struktura.Algorytmy.Ewolucyjny.PrzebiegAlgorytmu()).ZbudujAlgorytm(parametry, problem).Start();
        }
    }
}
