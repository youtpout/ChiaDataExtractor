using ChiaModels;
using System;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IRpcService
    {
        Task<RootBlockchainState> GetBlockchainState();
        Task<RootBlockRecord> GetBlockRecord(string hash);
        Task<RootWalletList> GetWallets();
        Task<RootFingerprint> GetFingerprint();
        Task<RootWalletSync> GetWalletSyncStatus();
        Task<RootWalletHeight> GetWalletHeighInfo();
        Task<RootNetworkInfo> GetNetworkInfo();
        Task<RootWalletBalance> GetWalletBalance(WalletBalanceParam parameters);

        Task<string> GetUrl(string url);
    }
}
