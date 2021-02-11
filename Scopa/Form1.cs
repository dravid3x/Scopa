using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scopa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Partita partitella = new Partita(3);
            for(int i = 0; i < 3; i++)
            {
                for(int x = 0; x < partitella.LunghezzaDeckGiocatore(i); x++)
                {
                    Console.WriteLine(partitella.LeggiCartaGiocatore(i, x).NCarta + " - " + partitella.LeggiCartaGiocatore(i, x).NSeme);
                }
                Console.WriteLine("-------------------");
            }
        }
    }
}
