using ChiaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IChiaInfoService
    {
        Task<FullNodeStatus> GetFullNodeStatus();
        Task<WalletInfo> GetWallet();
    }
}
