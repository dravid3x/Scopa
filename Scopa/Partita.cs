﻿using System;
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
        private const int nCarteDefaultGiocatore = 3, nCarteMaxTavolo = 9, defaultXOffset = 190, nCartaRiferimento = 0;
        private List<PunteggioRound> punteggi = new List<PunteggioRound>();
        private List<Mazzetto> mazziGiocatori = new List<Mazzetto>();
        private List<Mazzetto> preseGiocatori = new List<Mazzetto>();
        private List<Point[]> posizioniGiocatori = new List<Point[]>();
        private Mazzetto banco = new Mazzetto(0);
        private Mazzo mazzoPrincipale = new Mazzo();
        private Point[] posizioniTavolo = new Point[nCarteMaxTavolo];
        private int nGiocatori = 0, posBanco = 0;

        public Partita(int numGiocatori)
        {
            //Funzioni per la inizializzazione di una partita, come la generazione del mazzo principale, pescaggio delle carte verso i mazzi del giocatore e generazione delle posizioni disponibili nel tavolo
            mazzoPrincipale.RiempiMazzo();
            mazzoPrincipale.MescolaMazzo();
            nGiocatori = numGiocatori;
            for (int i = 0; i < nGiocatori; i++)
            {
                Mazzetto mazzetto = new Mazzetto(i);
                preseGiocatori.Add(mazzetto);
                mazziGiocatori.Add(mazzetto);

                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(i);   //Pesco dal mazzo per ogni giocatore (in questo caso per il computer e per il giocatore
            }

            GeneraPosizioniTavolo(nCarteMaxTavolo);
            GeneraPosizioniGiocatori();
        }

        #region Giocatore
        //Funzioni principali del giocatore come aggiunta di una carta, rimozione di una carta, restituzione di una carta in posizione nCarta con nGiocatore e restituzione dell numero di carte, pescaggio di una carta dal mazzo
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
        //Funzioni principali del banco/tavolo come aggiunta di una carta, rimozione di una carta, restituzione di una carta in posizione nCarta e restituzione dell numero di carte
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
            //Funzione di azzeramento dei giocatori con svuotamento delle liste e riinizializzazione
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

        public void TerminaMano()
        {
            //Funzione di reset della mano, con calcolo dei punteggi e reset dei giocatori e mazzo
            PunteggioRound punti = new PunteggioRound();
            punti.pGiocatore0 = CalcolaPunteggio(0);
            punti.pGiocatore1 = CalcolaPunteggio(1);
            punteggi.Add(punti);
            mazzoPrincipale.InizializzaMazzo();
            AzzeraGiocatori();
        }

        private void GeneraPosizioniTavolo(int nMaxPosizioni)
        {
            int nPosizioni = 0, posPosizioni = nMaxPosizioni / 2, larghezza = mazzoPrincipale.carta.Larghezza, altezza = mazzoPrincipale.carta.Altezza, offSet = defaultXOffset, incrementoOffset = 1;
            posizioniTavolo[nPosizioni++] = new Point((Form1.ActiveForm.ClientRectangle.Width / 2) - (larghezza / 2), (Form1.ActiveForm.ClientRectangle.Height / 2) - (altezza / 2));
            bool secondoPosizionamento = false;
            //Generazione delle posizioni alterne, centro, destra, sinistra ecc.
            while (nPosizioni < nMaxPosizioni)
            {
                if (nPosizioni % 2 == 0)
                {
                    posizioniTavolo[nPosizioni++] = new Point(posizioniTavolo[nCartaRiferimento].X - (offSet * incrementoOffset), posizioniTavolo[nCartaRiferimento].Y);
                    secondoPosizionamento = true;
                }
                else
                {
                    if (secondoPosizionamento)
                    {
                        secondoPosizionamento = false;
                        incrementoOffset++;
                    }
                    posizioniTavolo[nPosizioni++] = new Point(posizioniTavolo[nCartaRiferimento].X + (offSet * incrementoOffset), posizioniTavolo[nCartaRiferimento].Y);
                }
            }
            //for (int i = 0; i < nMaxPosizioni; i++) Console.WriteLine("X=" + posizioniTavolo[i].X + " Y=" + posizioniTavolo[i].Y);
            for (int i = 0; i < nMaxPosizioni; i++)
            {
                //Posizionamento all'interno del Form1
                Carta carta = new Carta(1, 2);
                carta.Location = posizioniTavolo[i];
                //Console.WriteLine("x: " + carta.Location.X + " Y: " + carta.Location.Y);
                Form1.ActiveForm.Controls.Add(carta);
            }
        }

        private void GeneraPosizioniGiocatori()
        {
            //Gestisco in maniera basica le posizioni delle carte in maniera basica non generalizzata come per il banco/tavolo per non esagerare e risparmiare tempo
            Point[] posizioniGiocatore = new Point[nCarteDefaultGiocatore];
            posizioniGiocatore[0] = new Point(posizioniTavolo[nCartaRiferimento].X, posizioniTavolo[nCartaRiferimento].Y + defaultXOffset * 2);
            posizioniGiocatore[1] = new Point(posizioniTavolo[nCartaRiferimento].X + defaultXOffset, posizioniTavolo[nCartaRiferimento].Y + defaultXOffset * 2);
            posizioniGiocatore[2] = new Point(posizioniTavolo[nCartaRiferimento].X - defaultXOffset, posizioniTavolo[nCartaRiferimento].Y + defaultXOffset * 2);
            posizioniGiocatori.Add(posizioniGiocatore);

            Point[] posizioniGiocatore2 = new Point[nCarteDefaultGiocatore];
            posizioniGiocatore2[0] = new Point(posizioniTavolo[nCartaRiferimento].X, posizioniTavolo[nCartaRiferimento].Y - defaultXOffset * 2);
            posizioniGiocatore2[1] = new Point(posizioniTavolo[nCartaRiferimento].X + defaultXOffset, posizioniTavolo[nCartaRiferimento].Y - defaultXOffset * 2);
            posizioniGiocatore2[2] = new Point(posizioniTavolo[nCartaRiferimento].X - defaultXOffset, posizioniTavolo[nCartaRiferimento].Y - defaultXOffset * 2);
            posizioniGiocatori.Add(posizioniGiocatore2);

            for (int i = 0; i < posizioniGiocatori.Count; i++)
            {
                for (int x = 0; x < nCarteDefaultGiocatore; x++)
                {
                    Carta temp = new Carta(1, 2);
                    temp.Location = posizioniGiocatori[i][x];
                    Form1.ActiveForm.Controls.Add(temp);
                }
            }
        }
    }
}
