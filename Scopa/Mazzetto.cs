using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopa
{
    public partial class Mazzetto
    {
        //Classe per la aggiunta di un oggetto Mazzetto, ovvero una sorta di mano utilizzabile per il banco, il giocatore, le carte prese dei giocatori

        public List<Carta> deck = new List<Carta>();
        private int nGiocatore;

        public Mazzetto(int numGiocatore)
        {
            nGiocatore = numGiocatore;
        }

        public int NGiocatore { get { return nGiocatore;} set { nGiocatore = value; } }
    }
}
