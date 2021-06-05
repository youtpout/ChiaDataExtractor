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
    [Route("[controller]")]
    public class ChiaController : ControllerBase
    {
        private readonly ILogger<ChiaController> _logger;
        private readonly IRpcService rpcService;

        public ChiaController(ILogger<ChiaController> logger, IRpcService rpcService)
        {
            _logger = logger;
            this.rpcService = rpcService;
        }

        [HttpGet]
        public async Task<FullNodeStatus> GetFullNodeStatus()
        {
            return await rpcService.GetFullNodeStatus();
        }

        [HttpGet("GetWallets")]
        public async Task<WalletInfo> GetWallet()
        {
            return await rpcService.GetWallet();
        }

        [HttpGet("url")]
        public async Task<string> GetUrl(string url)
        {
            return await rpcService.GetUrl(url);
        }
    }
}
