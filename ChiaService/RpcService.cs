using ChiaModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<FullNodeService> _logger;

        string urlFullNode;
        string urlFarmer;
        string urlHarvester;
        string urlWallet;
        // string urlDaemon;
        public RpcService(ICacheService cacheService, IConfiguration config, ILogger<FullNodeService> logger)
        {
            _logger = logger;
            this.cacheService = cacheService;
            urlFullNode = config["fullNode"];
            urlFarmer = config["farmer"];
            urlHarvester = config["harvester"];
            urlWallet = config["wallet"];
            // urlDaemon = config["daemon"];
        }

        public async Task<RootBlockRecord> GetBlockRecord(string hash)
        {
            var param = new BlockParam { header_hash = hash };
            var result = await Get<RootBlockRecord, BlockParam>($"{urlFullNode}get_block_record", param);
            return result;
        }

        public async Task<T> Get<T>(string url)
        {
            return await Get<T, object>(url, new object());
        }

        public async Task<T> Get<T, U>(string url, U data)
        {
            T result = default(T);
            var handler = new HttpClientHandler();
            var certificate = await cacheService.GetCertificate();
            handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;
            handler.ClientCertificates.Add(certificate);
            HttpClient client = new HttpClient(handler);

            HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();

            string log = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(log);

            // Deserialize the updated product from the response body.
            if (typeof(T) == typeof(string))
            {
                object dd = log;
                result = (T)dd;
            }
            else
            {
                result = await response.Content.ReadAsAsync<T>();
            }
            return result;
        }

        public async Task<string> GetUrl(string url)
        {
            return await Get<string>(url);
        }

        public async Task<RootBlockchainState> GetBlockchainState()
        {
            return await Get<RootBlockchainState>($"{urlFullNode}get_blockchain_state");
        }

        public async Task<RootWalletList> GetWallets()
        {
            return await Get<RootWalletList>($"{urlWallet}get_wallets");
        }

        public async Task<RootFingerprint> GetFingerprint()
        {
            return await Get<RootFingerprint>($"{urlWallet}get_public_keys");
        }

        public async Task<RootWalletSync> GetWalletSyncStatus()
        {
            return await Get<RootWalletSync>($"{urlWallet}get_sync_status");
        }

        public async Task<RootWalletHeight> GetWalletHeighInfo()
        {
            return await Get<RootWalletHeight>($"{urlWallet}get_height_info");
        }

        public async Task<RootNetworkInfo> GetNetworkInfo()
        {
            return await Get<RootNetworkInfo>($"{urlWallet}get_network_info");
        }

        public async Task<RootWalletBalance> GetWalletBalance(WalletBalanceParam parameters)
        {
            return await Get<RootWalletBalance, WalletBalanceParam>($"{urlWallet}get_wallet_balance ", parameters);
        }

        public async Task<RootConnection> GetFarmerConnection()
        {
            return await Get<RootConnection>($"{urlFarmer}get_connections");
        }

        public async Task<RootPlot> GetPlots()
        {
            return await Get<RootPlot>($"{urlHarvester}get_plots");
        }

        public async Task<FarmedAmount> GetFarmedAmount()
        {
            return await Get<FarmedAmount>($"{urlWallet}get_farmed_amount");
        }

        public async Task<RootBlockRecord> GetBlockRecordByHeight(long height)
        {
            var param = new BlockHeightParam { height = height };
            var result = await Get<RootBlockRecord, BlockHeightParam>($"{urlFullNode}get_block_record_by_height", param);
            return result;
        }
    }
}
