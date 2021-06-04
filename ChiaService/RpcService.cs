using ChiaModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public class RpcService : IRpcService
    {
        private readonly ICacheService cacheService;

        string urlFullNode;
        string urlFarmer;
        string urlHarvester;
        string urlWallet;
        public RpcService(ICacheService cacheService, IConfiguration config)
        {
            this.cacheService = cacheService;
            urlFullNode = config["fullNode"];
            urlFarmer = config["farmer"];
            urlHarvester = config["harvester"];
            urlWallet = config["wallet"];
        }

        public async Task<BlockchainInfo> GetBlockChainInfo()
        {
            var result = await Get<RootBlockchainState, object>($"{urlFullNode}get_blockchain_state", new object());
            var block = new BlockchainInfo();
            block.Status = result.blockchain_state.sync.synced ? "synced" : "syncing";
            block.Size = FormatBytes(result.blockchain_state.space);
            return block;
        }

        //public string GetFarmingInfo()
        //{
        //    return GetChiaInfos("farm summary").ToString();
        //}

        //public string GetWalletInfo()
        //{
        //    return GetChiaInfos("wallet show").ToString();
        //}
        public async Task<T> Get<T, U>(string url, U data)
        {

            var handler = new HttpClientHandler();
            var certificate = await cacheService.GetCertificate();
            handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;
            handler.ClientCertificates.Add(certificate);
            HttpClient client = new HttpClient(handler);

            HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            T result = await response.Content.ReadAsAsync<T>();
            string log = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(log);
            return result;
        }

        private string FormatBytes(decimal size)
        {
            string result = string.Empty;
            if (size < 0)
            {
                return "invalid";
            }
            var labels = new string[] { "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };
            decimal baseOctet = 1024;
            decimal value = size / baseOctet;
            foreach (var item in labels)
            {
                value /= baseOctet;
                if (value < baseOctet)
                {
                    return $"{value.ToString("F3")} {item}";
                }

            }
            return $"{value.ToString("F3")} {labels[labels.Length - 1]}";
        }
    }
}
