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
        private readonly IChiaInfoService infoService;
        private readonly IRpcService rpcService;

        public ChiaController(ILogger<ChiaController> logger, IChiaInfoService infoService, IRpcService rpcService)
        {
            _logger = logger;
            this.infoService = infoService;
            this.rpcService = rpcService;
        }

        [HttpGet("GetFullNodeStatus")]
        public async Task<FullNodeStatus> GetFullNodeStatus()
        {
            try
            {
                return await infoService.GetFullNodeStatus();
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
                return await infoService.GetWallet();
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
