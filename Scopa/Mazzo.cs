using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopa
{
    public partial class Mazzo
    {
        const int dimMazzo = 40;
        private int pos = 0;
        private Carta[] mazzo = new Carta[dimMazzo];








        public void riempiMazzo()
        {
            int pos = 0;
            for(int x = 1; x <= 4; x++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    mazzo[pos++] = new Carta(i, x);
                }
            }
        }
        public void mescolaMazzo()
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

        public Carta prossimaCarta()
        {
            if (pos == dimMazzo) pos = 0;
            return mazzo[pos++];
        }
    }
}
