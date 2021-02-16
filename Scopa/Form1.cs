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
            Partita partitella = new Partita(2);

            Carta cComputer1 = new Carta(1,2);
            Carta cComputer2 = new Carta(1,2);
            //Carta cComputer3 = new Carta(1,2);
            //Carta cComputer4 = new Carta(1,2);
            //Carta cComputer5 = new Carta(1,2);
            //Carta cComputer6 = new Carta(1,2);
            //Carta cComputer7 = new Carta(1,2);
            //Carta cComputer8 = new Carta(1,2);
            //Carta cComputer9 = new Carta(1,2);
            //int offSet = 190;
            cComputer1.Location = new Point((this.ClientRectangle.Width / 2) - (cComputer1.Size.Width / 2), (this.ClientRectangle.Height / 2) - (cComputer1.Size.Height / 2));
            //cComputer2.Location = new Point(cComputer1.Location.X + offSet, cComputer1.Location.Y);
            //cComputer3.Location = new Point(cComputer1.Location.X - offSet, cComputer1.Location.Y);
            //cComputer4.Location = new Point(cComputer1.Location.X + offSet * 2, cComputer1.Location.Y);
            //cComputer5.Location = new Point(cComputer1.Location.X - offSet * 2, cComputer1.Location.Y);
            //cComputer6.Location = new Point(cComputer1.Location.X + offSet * 3, cComputer1.Location.Y);
            //cComputer7.Location = new Point(cComputer1.Location.X - offSet * 3, cComputer1.Location.Y);
            //cComputer8.Location = new Point(cComputer1.Location.X + offSet * 4, cComputer1.Location.Y);
            //cComputer9.Location = new Point(cComputer1.Location.X - offSet * 4, cComputer1.Location.Y);

            //this.Controls.Add(cComputer1);
            //Console.WriteLine(cComputer1.Location.X + " " + cComputer1.Location.Y);
            //this.Controls.Add(cComputer2);
            //this.Controls.Add(cComputer3);
            //this.Controls.Add(cComputer4);
            //this.Controls.Add(cComputer5);
            //this.Controls.Add(cComputer6);
            //this.Controls.Add(cComputer7);
            //this.Controls.Add(cComputer8);
            //this.Controls.Add(cComputer9);
        }
    }
}