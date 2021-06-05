using ChiaModels;
using System;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IRpcService
    {
        Task<FullNodeStatus> GetFullNodeStatus();
        Task<RootBlockRecord> GetBlockRecord(string hash);
        Task<WalletInfo> GetWallet();

        Task<string> GetUrl(string url);

        //Task<string> GetFarmingInfo();
        //Task<string> GetWalletInfo();
    }
}
