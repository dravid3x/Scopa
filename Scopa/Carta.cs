using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Scopa
{
    public partial class Carta : System.Windows.Forms.PictureBox
    {
        //private int posInVet, nImmagine;
        private bool coperta = true, selezionata = false;
        private string nomeCopertina = "Back", posImmaginiDefault = "../../Img/", estensione = ".png";
        private const int larghezza = 128, altezza = 184;
        //Cuori = 0, Quadri = 1, Fiori = 2, Picche = 3
        private int nCarta = 0, nSeme = 0, nGiocatore = 0;

        public Carta(int nCartaP, int nSemeP/*int xP, int yP, int larghezza, int altezza, int nImmagine, int posInVet*/)
        {
            nCarta = nCartaP;
            nSeme = nSemeP;

            BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;             //Imposto il bordo incavato
            SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;    //Imposto lo stretch dell'immagine
            ImageLocation = posImmaginiDefault + nomeCopertina + estensione;    //Imposto l'immagine come retro carta
            //ImageLocation = posImmaginiDefault + nCarta + "-" + nSeme + estensione;
            Size = new System.Drawing.Size(larghezza, altezza);
            //BackColor = Color.FromArgb(0,0,0);

            #region Vecchio
            //this.Tag = nImmagine;                                                   //Inserisco nel tag il numero identificativo dell'immagine
            //this.posInVet = posInVet;                                               //Inserisco la posizione nel vettore nella sua variabile
            //this.nImmagine = nImmagine;
            #endregion
        }

        public void Gira()
        {
            //Funzione per "girare" la tessera cambiando la ImageLocation
            ImageLocation = coperta ? ImageLocation = posImmaginiDefault + nCarta + "-" + nSeme + estensione : posImmaginiDefault + nomeCopertina + estensione;
            coperta = !coperta;
        }
        //public int PosInVet { get { return posInVet; } }
        //public bool Cliccabile { get { return clickable; } set { clickable = value; } }
        /*public int NImmagine
        {
            get { return nImmagine; }
            set
            {
                nImmagine = value;
                this.ImageLocation = (clickable) ? posImmaginiDefault + nomeCopertina + estensione : posImmaginiDefault + nImmagine + estensione;   //Aggiorno pure la ImageLocation
                this.Tag = nImmagine;   //Aggiorno il tag
            }
        }*/
        /*public string PosImmaginiDefault
        {
            get { return posImmaginiDefault; }
            set
            {
                posImmaginiDefault = value;
                //Se le carte sono già state girate devo refresharle per cambiare il tema
                if (clickable == false) this.ImageLocation = posImmaginiDefault + nImmagine + estensione;
            }
        }*/
        public int NCarta { get { return nCarta; } set { nCarta = value; } }
        public int NSeme { get { return nSeme; } set { nSeme = value; } }
        public int Larghezza { get { return larghezza; } }
        public int Altezza { get { return altezza; } }
        public bool Coperta { get { return coperta; } }
        public bool Selezionata { get { return selezionata; } set { selezionata = value; } }
        public int NGiocatore { get { return nGiocatore; } set { if (value >= 0) nGiocatore = value; } }
    }
}
