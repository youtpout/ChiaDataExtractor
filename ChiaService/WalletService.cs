using ChiaModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public class WalletService : IWalletService
    {
        private readonly IRpcService rpcService;
        private readonly ILogger<WalletService> _logger;

        public WalletService(IRpcService rpcService, IConfiguration config, ILogger<WalletService> logger)
        {
            _logger = logger;
            this.rpcService = rpcService;
        }

        public async Task<WalletInfo> GetWallet()
        {
            WalletInfo infos = new WalletInfo();
            var fingerPrint = await rpcService.GetFingerprint();
            var walletInfo = await rpcService.GetWallets();
            var networkInfo = await rpcService.GetNetworkInfo();
            var syncInfo = await rpcService.GetWalletSyncStatus();
            var heightInfo = await rpcService.GetWalletHeighInfo();

            if (fingerPrint.public_key_fingerprints?.Count > 0)
            {
                infos.Fingerprint = fingerPrint.public_key_fingerprints[0];
            }

            infos.Synced = syncInfo.synced;
            infos.Syncing = syncInfo.syncing;
            infos.Height = heightInfo.height;
            if (walletInfo.success && walletInfo.wallets?.Count > 0)
            {
                WalletBalanceParam param = new WalletBalanceParam() { wallet_id = walletInfo.wallets[0].id };
                var balance = await rpcService.GetWalletBalance(param);
                infos.PendingBalance = balance.wallet_balance.pending_change;
                infos.TotalBalance = balance.wallet_balance.confirmed_wallet_balance;
                infos.SpendableBalance = balance.wallet_balance.spendable_balance;
            }

            return infos;
        }
    }
}
