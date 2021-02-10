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
            Mazzo mazzo = new Mazzo();
            mazzo.riempiMazzo();
            for (int i = 0; i < 40; i++)
            {
                Carta temp = mazzo.prossimaCarta();
                Console.WriteLine(temp.NCarta + " - " + temp.NSeme);
            }

            mazzo.mescolaMazzo();
            for (int i = 0; i < 40; i++)
            {
                Carta temp = mazzo.prossimaCarta();
                Console.WriteLine(temp.NCarta + " - " + temp.NSeme);
            }
        }
    }
}
