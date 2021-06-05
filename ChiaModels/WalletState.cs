using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
    public class WalletState
    {
    }

    public class RootFingerprint
    {
        public List<int> public_key_fingerprints { get; set; }
        public bool success { get; set; }
    }

    public class RootWalletHeight
    {
        public int height { get; set; }
        public bool success { get; set; }
    }

    public class RootWalletSync
    {
        public bool genesis_initialized { get; set; }
        public bool success { get; set; }
        public bool synced { get; set; }
        public bool syncing { get; set; }
    }

    public class RootWalletList
    {
        public List<WalletList> wallets { get; set; }
        public bool success { get; set; }
    }

    public class WalletList
    {
        public string data { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
    }

    public class WalletBalanceParam
    {
        public int wallet_id { get; set; }
    }

    public class RootWalletBalance
    {
        public WalletBalance wallet_balance { get; set; }
        public bool success { get; set; }
    }

    public class WalletBalance
    {
        public decimal confirmed_wallet_balance { get; set; }
        public decimal max_send_amount { get; set; }
        public decimal pending_change { get; set; }
        public decimal spendable_balance { get; set; }
        public decimal unconfirmed_wallet_balance { get; set; }
        public int wallet_id { get; set; }
        public bool success { get; set; }
    }

    public class RootNetworkInfo
    {
        public string network_name { get; set; }
        public string network_prefix { get; set; }
        public bool success { get; set; }
    }
}
