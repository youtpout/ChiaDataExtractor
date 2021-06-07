using ChiaModels;
using System;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IRpcService
    {
        Task<RootBlockchainState> GetBlockchainState();
        Task<RootBlockRecord> GetBlockRecord(string hash);
        Task<RootBlockRecord> GetBlockRecordByHeight(long height);
        Task<RootWalletList> GetWallets();
        Task<RootFingerprint> GetFingerprint();
        Task<RootWalletSync> GetWalletSyncStatus();
        Task<RootWalletHeight> GetWalletHeighInfo();
        Task<RootNetworkInfo> GetNetworkInfo();
        Task<RootWalletBalance> GetWalletBalance(WalletBalanceParam parameters);
        Task<RootConnection> GetFarmerConnection();
        Task<RootPlot> GetPlots();
        Task<FarmedAmount> GetFarmedAmount();

        Task<string> GetUrl(string url);
    }
}
