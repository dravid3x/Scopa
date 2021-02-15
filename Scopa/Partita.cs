using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Scopa
{
    public partial class Partita
    {
        private struct PunteggioRound { public int pGiocatore0; public int pGiocatore1; }
        private const int nCarteDefaultGiocatore = 3, nCarteMaxTavolo = 9, defaultXOffset = 190;
        private List<PunteggioRound> punteggi = new List<PunteggioRound>();
        private List<Mazzetto> mazziGiocatori = new List<Mazzetto>();
        private List<Mazzetto> preseGiocatori = new List<Mazzetto>();
        private Mazzetto banco = new Mazzetto(0);
        private Mazzo mazzoPrincipale = new Mazzo();
        private Point[] posizioniTavolo = new Point[nCarteMaxTavolo];
        private int nGiocatori;

        public Partita(int numGiocatori)
        {
            mazzoPrincipale.RiempiMazzo();
            mazzoPrincipale.MescolaMazzo();
            nGiocatori = numGiocatori;
            for (int i = 0; i < nGiocatori; i++)
            {
                Mazzetto mazzetto = new Mazzetto(i);
                preseGiocatori.Add(mazzetto);
                mazziGiocatori.Add(mazzetto);

                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(i);
            }

            GeneraPosizioniTavolo(nCarteMaxTavolo);
        }

        #region Giocatore
        public void AggiungiCartaGiocatore(int nGiocatore, Carta carta)
        {
            mazziGiocatori[nGiocatore].deck.Add(carta);
        }

        public void RimuoviCartaGiocatore(int nGiocatore, Carta carta)
        {
            mazziGiocatori[nGiocatore].deck.Remove(carta);
        }

        public Carta LeggiCartaGiocatore(int nGiocatore, int nCarta)
        {
            return mazziGiocatori[nGiocatore].deck[nCarta];
        }

        public int LunghezzaDeckGiocatore(int nGiocatore)
        {
            return mazziGiocatori[nGiocatore].deck.Count;
        }

        public void PescaDaMazzo(int nGiocatore)
        {
            mazziGiocatori[nGiocatore].deck.Add(mazzoPrincipale.PescaCarta());
        }
        #endregion

        #region Banco
        public void AggiungiCartaBanco(Carta carta)
        {
            banco.deck.Add(carta);
        }

        public void RimuoviCartaBanco(Carta carta)
        {
            banco.deck.Remove(carta);
        }

        public Carta LeggiCartaBanco(int nCarta)
        {
            return banco.deck[nCarta];
        }

        public int LunghezzaDeckBanco()
        {
            return banco.deck.Count;
        }
        #endregion

        public int NumeroGiocatori { get { return NumeroGiocatori; } set { NumeroGiocatori = value; } }

        public int DimMazzoPrincipale { get { return mazzoPrincipale.DimMazzo(); } }

        private void AzzeraGiocatori()
        {
            mazziGiocatori.Clear();
            preseGiocatori.Clear();
            mazzoPrincipale.InizializzaMazzo();
            for (int i = 0; i < nGiocatori; i++)
            {
                Mazzetto mazzetto = new Mazzetto(i);
                mazziGiocatori.Add(mazzetto);
                preseGiocatori.Add(mazzetto);

                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(i);
            }
        }

        private int CalcolaPunteggio(int nGiocatore)
        {
            //Calcolo punteggio del giocatore nGiocatore
            return 0;
        }

        public void TerminaPartita()
        {
            PunteggioRound punti = new PunteggioRound();
            punti.pGiocatore0 = CalcolaPunteggio(0);
            punti.pGiocatore1 = CalcolaPunteggio(1);
            punteggi.Add(punti);
            mazzoPrincipale.InizializzaMazzo();
            AzzeraGiocatori();
        }

        private void GeneraPosizioniTavolo(int nMaxPosizioni)
        {
            int nPosizioni = 0, posPosizioni = nMaxPosizioni / 2, larghezza = mazzoPrincipale.carta.Larghezza, altezza = mazzoPrincipale.carta.Altezza, offSet = defaultXOffset, incrementoOffset = 2;
            bool incrementaOffSet = false;
            while (nPosizioni < nMaxPosizioni)
            {
                if (nPosizioni % 2 == 0 && nPosizioni != 0)
                {
                    posizioniTavolo[nPosizioni++] = new Point(((Form1.ActiveForm.ClientRectangle.Width / 2) - (larghezza / 2)) + offSet, (Form1.ActiveForm.ClientRectangle.Height / 2) - (altezza / 2));
                }
                else
                {
                    posizioniTavolo[nPosizioni++] = new Point(((Form1.ActiveForm.ClientRectangle.Width / 2) + (larghezza / 2)) + offSet, (Form1.ActiveForm.ClientRectangle.Height / 2) - (altezza / 2));
                    incrementaOffSet = true;
                }
                if (incrementaOffSet)
                {
                    offSet = defaultXOffset * incrementoOffset++;
                    incrementaOffSet = false;
                }
            }
            for (int i = 0; i < nMaxPosizioni; i++) Console.WriteLine("X=" + posizioniTavolo[i].X + " Y=" + posizioniTavolo[i].Y);
            //for(int i = 0; i < nMaxPosizioni; i++)
            //{
            //    Carta carta = new Carta(1, 2);
            //    carta.Location = posizioniTavolo[i];
            //    Form1.ActiveForm.Controls.Add(carta);
            //}

            Carta carta = new Carta(1, 2);
            carta.Location = posizioniTavolo[i];
            Form1.ActiveForm.Controls.Add(carta);
        }
    }
}
