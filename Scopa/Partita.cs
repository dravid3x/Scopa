using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopa
{
    public partial class Partita
    {
        private const int nCarteDefaultGiocatore = 3;
        private List<Mazzetto> mazziGiocatori = new List<Mazzetto>();
        private Mazzetto banco = new Mazzetto(0);
        private Mazzo mazzoPrincipale = new Mazzo();
        private int nGiocatori;

        public Partita(int numGiocatori)
        {
            mazzoPrincipale.RiempiMazzo();
            mazzoPrincipale.MescolaMazzo();
            nGiocatori = numGiocatori;
            for (int i = 1; i <= nGiocatori; i++)
            {
                Mazzetto mazzetto = new Mazzetto(i);
                mazziGiocatori.Add(mazzetto);
                for (int x = 0; x < nCarteDefaultGiocatore; x++)
                {
                    Carta carta = new Carta(x + 1, i);
                    mazziGiocatori[i - 1].deck.Add(carta);
                }
            }
        }

        public Carta LeggiMazzo(int nPlayer, int posDeck)
        {
            return mazziGiocatori[nPlayer].deck[posDeck];
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
    }
}
