using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
    public class WalletInfo
    {
        public int Height { get; set; }
        public bool Synced { get; set; }
        public int Fingerprint { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal PendingBalance { get; set; }
        public decimal SpendableBalance { get; set; }
    }
}
