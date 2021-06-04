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
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRpcService cliService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRpcService cliService)
        {
            _logger = logger;
            this.cliService = cliService;
        }

        [HttpGet]
        public async Task<BlockchainInfo> Get()
        {
            var rng = new Random();
          return await cliService.GetBlockChainInfo();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)],
            //    Data = r.Status
            //})
            //.ToArray();
        }
    }
}
