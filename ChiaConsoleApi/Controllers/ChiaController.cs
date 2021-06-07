using ChiaModels;
using ChiaService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChiaConsoleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChiaController : ControllerBase
    {
        private readonly ILogger<ChiaController> _logger;
        private readonly IFullNodeService fullNodeService;
        private readonly IFarmerService farmerService;
        private readonly IWalletService walletService;
        private readonly IRpcService rpcService;

        public ChiaController(ILogger<ChiaController> logger, IFullNodeService fullNodeService, IFarmerService farmerService, IWalletService walletService, IRpcService rpcService)
        {
            _logger = logger;
            this.fullNodeService = fullNodeService;
            this.farmerService = farmerService;
            this.walletService = walletService;
            this.rpcService = rpcService;
        }

        [HttpGet("GetFullNodeStatus")]
        public async Task<FullNodeStatus> GetFullNodeStatus()
        {
            try
            {
                return await fullNodeService.GetFullNodeStatus();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("GetWallet")]
        public async Task<WalletInfo> GetWallet()
        {
            try
            {
                return await walletService.GetWallet();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("GetFarmingInfo")]
        public async Task<FarmingInfo> GetFarmingInfo()
        {
            try
            {
                return await farmerService.GetFarmingInfo();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("url")]
        public async Task<string> GetUrl(string url)
        {
            try
            {
                return await rpcService.GetUrl(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
