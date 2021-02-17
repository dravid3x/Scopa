using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Scopa
{
    public partial class Mazzo
    {
        private const int dimMazzo = 40;
        private int pos = 0;
        private Carta[] mazzo = new Carta[dimMazzo];
        private bool SelezionataGiocatore = false;
        private Carta cartaSelezionata = new Carta(0, 0);

        public void RiempiMazzo()
        {
            int pos = 0;
            for(int x = 0; x < 4; x++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    mazzo[pos] = new Carta(i, x);
                    mazzo[pos++].Click += new EventHandler(ClickCarta);
                }
            }
        }
        public void MescolaMazzo()
        {
            Random rand = new Random();
            for(int i = 0; i < dimMazzo; i++)
            {
                int posRand = rand.Next(40);
                Carta tempCarta = mazzo[posRand];
                mazzo[posRand] = mazzo[i];
                mazzo[i] = tempCarta;
            }
        }

        public Carta carta { get { return mazzo[pos]; } }

        public Carta ProssimaCarta()
        {
            //Funzione che restituisce la carta nella posizione attuale e aumenta l'indice
            if (pos == dimMazzo) pos = 0;
            return mazzo[pos++];
        }

        public Carta PescaCarta()
        {
            return mazzo[pos++];
        }

        public int DimMazzo() { return dimMazzo - pos; }

        public void InizializzaMazzo()
        {
            pos = 0;
            RiempiMazzo();
            MescolaMazzo();
        }

        public void ImpostaPosizioneCarta(int nCarta, Point posizione)
        {
            mazzo[nCarta].Location = posizione;
        }

        public void PosizionaCarte()
        {
            int dim = dimMazzo - pos;
            for(int i = 0; i < dim; i++)
            {
                Form1.ActiveForm.Controls.Add(mazzo[i]);
            }
        }

        private void ClickCarta(object sender, EventArgs e)
        {
            //Per ora abilito la selezione solo sulle carte del giocatore con controllo manuale
            if (!SelezionataGiocatore && ((Carta)sender).NGiocatore == 1)
            {
                ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y - 30);
                cartaSelezionata = ((Carta)sender);
                SelezionataGiocatore = true;
            }
            else if (((Carta)sender).NGiocatore == -1)
            {
                ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y - 30);
            }
        }

        public Carta CartaSelezionata { get { return cartaSelezionata; } }
    }
}
