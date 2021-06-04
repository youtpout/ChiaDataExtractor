using ChiaModels;
using System;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IRpcService
    {
        Task<BlockchainInfo> GetBlockChainInfo();
        //Task<string> GetFarmingInfo();
        //Task<string> GetWalletInfo();
    }
}
