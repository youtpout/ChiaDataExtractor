﻿using ChiaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface IWalletService
    {
        Task<WalletInfo> GetWallet();
    }
}
