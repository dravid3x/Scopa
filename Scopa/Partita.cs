using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Scopa
{
    public partial class Partita
    {
        private struct PunteggioRound { public int pGiocatore0; public int pGiocatore1; }
        private struct PosizioniCarte { public Point posizione; public bool utilizzata; }
        private const int nCarteDefaultGiocatore = 3, nCarteMaxTavolo = 9, defaultXOffset = 190, nCartaRiferimento = 0, nCarteInizialiTavolo = 4, offSetSelezione = 30, offSetScopa = 15;   //Costanti che servono alla limitazione e gesitone del gioco (tutte modificabili meno nCartaRiferimento
        private readonly int larghezzaForm = Form1.ActiveForm.ClientRectangle.Width, altezzaForm = Form1.ActiveForm.ClientRectangle.Height;      //Vengono salvate la larghezza e altezza del form
        private List<PunteggioRound> punteggi = new List<PunteggioRound>();
        private List<Mazzetto> mazziGiocatori = new List<Mazzetto>();
        private List<Mazzetto> preseGiocatori = new List<Mazzetto>();
        private List<Point[]> posizioniGiocatori = new List<Point[]>();
        private Mazzetto banco = new Mazzetto(0);
        private Mazzo mazzoPrincipale = new Mazzo();
        private PosizioniCarte[] posizioniTavolo = new PosizioniCarte[nCarteMaxTavolo];
        private Point posizioneMazzo = new Point(defaultXOffset, defaultXOffset / 2 - defaultXOffset / 4);
        private Point posizioneMazzettoComputer = new Point(defaultXOffset * 2, defaultXOffset / 2 - defaultXOffset / 4);
        private Point posizioneMazzettoGiocatore = new Point(defaultXOffset * 2, defaultXOffset * 4 + defaultXOffset / 4);
        private int nGiocatori = 0, maxPunti = 21, nScopaGiocatore = 0, nScopaComputer = 0, maxScopa = 5;
        private bool turnoGiocatore = true, scopa = false;

        //Variabili per funzione click delle carte
        private Carta cartaVuota = new Carta(0, 0);
        private Carta cartaSelezionata = new Carta(0, 0);
        private List<Carta> carteSelezionate = new List<Carta>();
        private int sommaCarteScelte = 0;
        private bool assoSelezionato = false;

        public Partita(int numGiocatori)
        {
            //Funzioni per la inizializzazione di una partita, come la generazione del mazzo principale, pescaggio delle carte verso i mazzi del giocatore e generazione delle posizioni disponibili nel tavolo
            mazzoPrincipale.InizializzaMazzo();
            GeneraPosizioniTavolo(nCarteMaxTavolo);
            GeneraPosizioniGiocatori();
            //Posizionamento nel campo
            for (int i = 0; i < mazzoPrincipale.DimMazzo(); i++) mazzoPrincipale.ImpostaPosizioneCarta(i, posizioneMazzo);
            mazzoPrincipale.PosizionaCarte();
            nGiocatori = numGiocatori;
            for (int i = 0; i < nGiocatori; i++)
            {
                Mazzetto mazzetto = new Mazzetto(i);
                preseGiocatori.Add(mazzetto);
                mazzetto = new Mazzetto(i);
                mazziGiocatori.Add(mazzetto);
            }
            //Pesco dal mazzo per ogni giocatore (in questo caso per il computer e per il giocatore. iniziaComputer gestisce chi inizia
            if (!turnoGiocatore)
            {
                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(0);
                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(1);
            }
            else
            {
                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(1);
                for (int x = 0; x < nCarteDefaultGiocatore; x++) PescaDaMazzo(0);
            }
            //Posiziona carte tavolo
            for (int i = 0; i < nCarteInizialiTavolo; i++) PescaDaMazzoBanco();

            //ClickCarta(mazziGiocatori[0].deck[0], EventArgs.Empty);
        }

        #region Giocatore
        //Funzioni principali del giocatore come aggiunta di una carta, rimozione di una carta, restituzione di una carta in posizione nCarta con nGiocatore e restituzione dell numero di carte, pescaggio di una carta dal mazzo

        //public void AggiungiCartaGiocatore(int nGiocatore, Carta carta)
        //{
        //    mazziGiocatori[nGiocatore].deck.Add(carta);

        //}

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
            //Funzione che cambia la posizione delle carte per darle ai giocatori dopo aver pescato la carta
            mazziGiocatori[nGiocatore].deck.Add(mazzoPrincipale.PescaCarta());
            int tempPos = mazziGiocatori[nGiocatore].deck.Count - 1;
            mazziGiocatori[nGiocatore].deck[tempPos].Click += new EventHandler(ClickCarta);
            mazziGiocatori[nGiocatore].deck[tempPos].NGiocatore = nGiocatore;

            mazziGiocatori[nGiocatore].deck[tempPos].Location = posizioniGiocatori[nGiocatore][mazziGiocatori[nGiocatore].deck.Count - 1];
            if (nGiocatore == 1) mazziGiocatori[nGiocatore].deck[tempPos].Gira();
        }

        #endregion

        #region Banco
        //Funzioni principali del banco/tavolo come aggiunta di una carta, rimozione di una carta, restituzione di una carta in posizione nCarta e restituzione dell numero di carte
        public void PescaDaMazzoBanco()
        {
            banco.deck.Add(mazzoPrincipale.PescaCarta());
            int posBanco = banco.deck.Count - 1;
            banco.deck[posBanco].Click += new EventHandler(ClickCarta);
            int posLibera = PosizioneLiberaBanco();
            banco.deck[posBanco].Location = posizioniTavolo[posLibera].posizione;
            posizioniTavolo[posLibera].utilizzata = true;
            posizioniTavolo[posBanco].utilizzata = true;
            banco.deck[posBanco].NGiocatore = -1;
            banco.deck[posBanco].Gira();
        }

        private int PosizioneLiberaBanco()
        {
            //Ricerco la prima posizione disponibile nel tavolo
            bool trovato = false; int i = 0;
            while(!trovato && i < nCarteMaxTavolo)
            {
                if (!posizioniTavolo[i].utilizzata) return i;
                else i++;
            }
            return -1;
        }

        private void RimuoviCartaBanco(Carta carta)
        {
            banco.deck.Remove(carta);
            RiabilitaPosizioneBanco(carta);
        }

        private void RiabilitaPosizioneBanco(Carta carta)
        {
            bool trovato = false; int i = 0;
            while (!trovato && i < nCarteMaxTavolo)
            {
                if (posizioniTavolo[i].posizione == carta.Location || posizioniTavolo[i].posizione.X == carta.Location.X && posizioniTavolo[i].posizione.Y == (carta.Location.Y + offSetSelezione))
                {
                    posizioniTavolo[i].utilizzata = false;
                    trovato = true;
                }
                i++;
            }
        }

        private void AggiuntiCartaBanco(Carta carta)
        {
            int posLibera = PosizioneLiberaBanco();
            carta.Location = posizioniTavolo[posLibera].posizione;
            posizioniTavolo[posLibera].utilizzata = true;
            banco.deck.Add(carta);
        }

        //private Carta LeggiCartaBanco(int nCarta)
        //{
        //    return banco.deck[nCarta];
        //}

        //private int LunghezzaDeckBanco()
        //{
        //    return banco.deck.Count;
        //}

        #endregion

        private void SpostaInMazzetto(Carta carta)
        {
            preseGiocatori[carta.NGiocatore].deck.Add(carta);
            if (carta == cartaSelezionata || carta.NGiocatore == 0) mazziGiocatori[carta.NGiocatore].deck.Remove(carta);
            else RimuoviCartaBanco(carta);
            if (!scopa && carta.NGiocatore == 1) carta.Gira();
            carta.Location = (carta.NGiocatore == 0) ? (scopa) ? new Point(posizioneMazzettoComputer.X, posizioneMazzettoComputer.Y + (offSetScopa * (++nScopaComputer - maxScopa))) : posizioneMazzettoComputer : (scopa) ? new Point(posizioneMazzettoGiocatore.X, posizioneMazzettoGiocatore.Y - (-offSetScopa * (++nScopaGiocatore - maxScopa))) : posizioneMazzettoGiocatore;
        }

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
            nScopaGiocatore = 0;
            nScopaComputer = 0;
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
            nScopaComputer = 0;
            nScopaGiocatore = 0;
            AzzeraGiocatori();
        }

        private void GeneraPosizioniTavolo(int nMaxPosizioni)
        {
            int nPosizioni = 0, posPosizioni = nMaxPosizioni / 2, larghezza = mazzoPrincipale.carta.Larghezza, altezza = mazzoPrincipale.carta.Altezza, offSet = defaultXOffset, incrementoOffset = 1;
            posizioniTavolo[nPosizioni++].posizione = new Point((larghezzaForm / 2) - (larghezza / 2), (altezzaForm / 2) - (altezza / 2));
            bool secondoPosizionamento = false;
            //Generazione delle posizioni alterne, centro, destra, sinistra ecc.
            while (nPosizioni < nMaxPosizioni)
            {
                if (nPosizioni % 2 == 0)
                {
                    posizioniTavolo[nPosizioni++].posizione = new Point(posizioniTavolo[nCartaRiferimento].posizione.X - (offSet * incrementoOffset), posizioniTavolo[nCartaRiferimento].posizione.Y);
                    secondoPosizionamento = true;
                }
                else
                {
                    if (secondoPosizionamento)
                    {
                        secondoPosizionamento = false;
                        incrementoOffset++;
                    }
                    posizioniTavolo[nPosizioni++].posizione = new Point(posizioniTavolo[nCartaRiferimento].posizione.X + (offSet * incrementoOffset), posizioniTavolo[nCartaRiferimento].posizione.Y);
                }
            }
        }

        private void GeneraPosizioniGiocatori()
        {
            //Gestisco in maniera basica le posizioni delle carte in maniera basica non generalizzata come per il banco/tavolo per non esagerare e risparmiare tempo
            Point[] posizioniGiocatore = new Point[nCarteDefaultGiocatore];
            posizioniGiocatore[0] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X, posizioniTavolo[nCartaRiferimento].posizione.Y - defaultXOffset * 2);
            posizioniGiocatore[1] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X + defaultXOffset, posizioniTavolo[nCartaRiferimento].posizione.Y - defaultXOffset * 2);
            posizioniGiocatore[2] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X - defaultXOffset, posizioniTavolo[nCartaRiferimento].posizione.Y - defaultXOffset * 2);
            posizioniGiocatori.Add(posizioniGiocatore);

            Point[] posizioniGiocatore2 = new Point[nCarteDefaultGiocatore];
            posizioniGiocatore2[0] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X, posizioniTavolo[nCartaRiferimento].posizione.Y + defaultXOffset * 2);
            posizioniGiocatore2[1] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X + defaultXOffset, posizioniTavolo[nCartaRiferimento].posizione.Y + defaultXOffset * 2);
            posizioniGiocatore2[2] = new Point(posizioniTavolo[nCartaRiferimento].posizione.X - defaultXOffset, posizioniTavolo[nCartaRiferimento].posizione.Y + defaultXOffset * 2);
            posizioniGiocatori.Add(posizioniGiocatore2);
        }

        private void ClickCarta(object sender, EventArgs e)
        {
            //Per ora abilito la selezione solo sulle carte del giocatore
            if (turnoGiocatore)
            {
                if (((Carta)sender).NGiocatore == 1 && cartaSelezionata != ((Carta)sender))     //Situazione nella quale la carta selezionata è del giocatore e non è la stessa già selezionata
                {
                    if (cartaSelezionata == cartaVuota)
                    {
                        ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y - offSetSelezione);
                        cartaSelezionata = ((Carta)sender);
                        cartaSelezionata.SelezionataBanco = true;
                    }
                    else
                    {
                        AbbassaTuttoBanco();
                        sommaCarteScelte = 0;
                        cartaSelezionata.Location = new Point(cartaSelezionata.Location.X, cartaSelezionata.Location.Y + offSetSelezione);
                        ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y - offSetSelezione);
                        cartaSelezionata = ((Carta)sender);
                        cartaSelezionata.SelezionataBanco = true;
                    }
                    if (((Carta)sender).NCarta == 1) assoSelezionato = true;
                }
                //Selezionata carta del banco
                else if (((Carta)sender).NGiocatore == -1 && cartaSelezionata.NCarta != cartaVuota.NCarta)
                {
                    if (((Carta)sender).SelezionataBanco)
                    {
                        ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y + offSetSelezione);
                        carteSelezionate.Remove(((Carta)sender));
                        sommaCarteScelte -= ((Carta)sender).NCarta;
                        ((Carta)sender).SelezionataBanco = false;
                    }
                    //Funzione Asso piglia Tutto
                    else if (assoSelezionato)
                    {
                        cartaSelezionata.Enabled = false;
                        SpostaInMazzetto(cartaSelezionata);
                        cartaSelezionata = new Carta(0, 0);
                        int dimBanco = banco.deck.Count, x = 0;
                        for (int i = 0; i < dimBanco; i++)
                        {
                            carteSelezionate.Add(banco.deck[x]);
                            carteSelezionate[carteSelezionate.Count - 1].NGiocatore = 1;
                            carteSelezionate[i].SelezionataBanco = true;
                            carteSelezionate[i].Enabled = false;
                            SpostaInMazzetto(carteSelezionate[i]);
                        }
                        carteSelezionate.Clear();
                        sommaCarteScelte = 0;
                        assoSelezionato = false;
                        turnoGiocatore = false;
                    }
                    else if ((((Carta)sender).NCarta + sommaCarteScelte <= cartaSelezionata.NCarta))
                    {
                        ((Carta)sender).Location = new Point(((Carta)sender).Location.X, ((Carta)sender).Location.Y - offSetSelezione);
                        ((Carta)sender).SelezionataBanco = true;
                        carteSelezionate.Add(((Carta)sender));
                        sommaCarteScelte += ((Carta)sender).NCarta;
                    }
                    //Rimozione delle carte
                    if (sommaCarteScelte == cartaSelezionata.NCarta)
                    {
                        //Spostiamo le carte del banco selezionate nel mazzetto del giocatore che le ha prese
                        for (int i = 0; i < carteSelezionate.Count; i++)
                        {
                            carteSelezionate[i].NGiocatore = ((turnoGiocatore) ? 1 : 0);
                            carteSelezionate[i].Enabled = false;
                            SpostaInMazzetto(carteSelezionate[i]);
                        }
                        carteSelezionate.Clear();
                        sommaCarteScelte = 0;

                        cartaSelezionata.Enabled = false;
                        //Controllo scopa
                        if (banco.deck.Count == 0)
                        {
                            scopa = true;
                            SpostaInMazzetto(cartaSelezionata);
                            scopa = false;
                        }
                        else SpostaInMazzetto(cartaSelezionata);
                        cartaSelezionata = new Carta(0, 0);
                        turnoGiocatore = false;

                    }
                }
                else if (cartaSelezionata == ((Carta)sender))       //Situazione nella quale seleziono la carta che ho già scelto, ho deciso che facendo in questo modo la carta vada posizionata sul banco
                {
                    AbbassaTuttoBanco();    //Abbasso tutte le carte selezionate
                    mazziGiocatori[cartaSelezionata.NGiocatore].deck.Remove(cartaSelezionata);
                    cartaSelezionata.NGiocatore = -1;
                    AggiuntiCartaBanco(cartaSelezionata);
                    cartaSelezionata = cartaVuota;
                    turnoGiocatore = false;
                }
                if (!turnoGiocatore) GiocataComputer();
            }
            else
            {
                SpostaInMazzetto(((Carta)sender));
                turnoGiocatore = true;
            }
            //Controllo se le carte di tutti i giocatori sono finite
            bool finite = true;
            for(int i = 0; i < mazziGiocatori.Count; i++)
            {
                if (mazziGiocatori[i].deck.Count != 0) finite = false;
            }
            if(finite) //Se le carte sono finite nelle mani dei giocatori sono finite ridò le carte
            {
                for (int i = 0; i < nCarteDefaultGiocatore; i++) PescaDaMazzo(0);
                for (int i = 0; i < nCarteDefaultGiocatore; i++) PescaDaMazzo(1);
            }
        }

        private void AbbassaTuttoBanco()
        {
            //Funzione che abbassa tutte le eventuali carte selezionate
            for (int i = 0; i < carteSelezionate.Count; i++)
            {
                if (carteSelezionate[i].SelezionataBanco)
                {
                    carteSelezionate[i].SelezionataBanco = false;
                    carteSelezionate[i].Location = new Point(carteSelezionate[i].Location.X, carteSelezionate[i].Location.Y + offSetSelezione);
                    carteSelezionate.RemoveAt(i);
                }
            }
        }

        private void GiocataComputer()
        {
            //Funzione che esegue il click da parte del computer su una determinata carta
            ClickCarta(mazziGiocatori[0].deck[mazziGiocatori[0].deck.Count - 1], EventArgs.Empty);
        }

        public int MaxPunti { get { return maxPunti; } set { if (value >= 1) maxPunti = value; } }

    }
}
