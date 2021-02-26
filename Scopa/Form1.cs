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
            this.BackgroundImage = new Bitmap("../../Img/GreenTable.jpg");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Partita partitella = new Partita(2, this);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
