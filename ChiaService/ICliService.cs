using ChiaModels;
using System;

namespace ChiaService
{
    public interface ICliService
    {
        BlockchainInfo GetBlockChainInfo();
        string GetFarmingInfo();
        string GetWalletInfo();
    }
}
