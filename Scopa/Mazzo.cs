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
        private Carta[] mazzo = new Carta[dimMazzo];





        


        public void riempiMazzo()
        {
            for(int i = 0; i < dimMazzo; i++)
            {
                mazzo[i] = new Carta(1, 1);
                Console.WriteLine(mazzo[i].NCarta + " - " + mazzo[i].NSeme);
            }
        }
        public void mescolaMazzo()
        {

        }

    }
}
