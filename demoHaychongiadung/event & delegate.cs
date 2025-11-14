using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoHaychongiadung
{
    // ==================== EVENT & DELEGATE ====================
    public delegate void WinnerEventHandler(object sender, WinnerEventArgs e);

    public class WinnerEventArgs : EventArgs
    {
        public Player Winner { get; private set; }

        public WinnerEventArgs(Player winner)
        {
            this.Winner = winner;
        }
    }
}
