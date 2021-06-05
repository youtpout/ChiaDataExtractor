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
        // string urlDaemon;
        public RpcService(ICacheService cacheService, IConfiguration config)
        {
            this.cacheService = cacheService;
            urlFullNode = config["fullNode"];
            urlFarmer = config["farmer"];
            urlHarvester = config["harvester"];
            urlWallet = config["wallet"];
            // urlDaemon = config["daemon"];
        }

        public async Task<FullNodeStatus> GetFullNodeStatus()
        {
            var blockchainState = await Get<RootBlockchainState>($"{urlFullNode}get_blockchain_state");
            var nodeStatus = new FullNodeStatus();
            if (blockchainState.success)
            {
                var state = blockchainState.blockchain_state;
                if (state.sync.sync_mode)
                {
                    nodeStatus.Status = $"Syncing {state.sync.sync_tip_height}/{state.sync.sync_progress_height}";
                }
                else
                {
                    nodeStatus.Status = state.sync.synced ? "Full node Synced" : " Not Synced";
                }

                try
                {
                    var networkInfo = await Get<RootNetworkInfo>($"{urlWallet}get_network_info");
                    nodeStatus.NetworkName = networkInfo.network_name;
                }
                catch (Exception)
                {

                    throw;
                }

                //nodeStatus.ConnectionStatus =
                nodeStatus.EstimatedNetworkSpace = FormatBytes(state.space);
                nodeStatus.Difficulty = state.difficulty;
                nodeStatus.PeakHeight = state.peak.height;
                if (state.peak.timestamp.HasValue)
                {
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(state.peak.timestamp.Value);
                    nodeStatus.PeakTime = dateTimeOffset.DateTime.ToString("F");
                }
                else
                {
                    try
                    {
                        long timestamp = await GetDateTimeFromBlock(state.peak.header_hash);
                        if (timestamp > 0)
                        {
                            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                            nodeStatus.PeakTime = dateTimeOffset.DateTime.ToString("F");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }


                nodeStatus.TotalIterations = state.peak.total_iters;
            }

            return nodeStatus;
        }

        public async Task<WalletInfo> GetWallet()
        {
            WalletInfo infos = new WalletInfo();
            var fingerPrint = await Get<RootFingerprint>($"{urlWallet}get_public_keys");
            var walletInfo = await Get<RootWalletList>($"{urlWallet}get_wallets");
            var networkInfo = await Get<RootNetworkInfo>($"{urlWallet}get_network_info");
            var syncInfo = await Get<RootWalletSync>($"{urlWallet}get_sync_status");
            var heightInfo = await Get<RootWalletHeight>($"{urlWallet}get_height_info");

            if (fingerPrint.public_key_fingerprints?.Count > 0)
            {
                infos.Fingerprint = fingerPrint.public_key_fingerprints[0];
            }

            infos.Synced = syncInfo.synced;
            infos.Height = heightInfo.height;
            if (walletInfo.success && walletInfo.wallets?.Count > 0)
            {
                WalletBalanceParam param = new WalletBalanceParam() { wallet_id = walletInfo.wallets[0].id };
                var balance = await Get<RootWalletBalance, WalletBalanceParam>($"{urlWallet}get_wallet_balance ", param);
                infos.PendingBalance = balance.wallet_balance.pending_change;
                infos.TotalBalance = balance.wallet_balance.confirmed_wallet_balance;
                infos.SpendableBalance = balance.wallet_balance.spendable_balance;
            }


            return infos;
        }


        private async Task<long> GetDateTimeFromBlock(string hash)
        {
            var blockRecord = await GetBlockRecord(hash);
            if (blockRecord.success)
            {
                if (blockRecord.block_record.timestamp.HasValue)
                {
                    return blockRecord.block_record.timestamp.Value;
                }
                else
                {
                    return await GetDateTimeFromBlock(blockRecord.block_record.prev_hash);
                }

            }
            return 0;
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
            Debug.WriteLine(log);
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

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public async Task<string> GetUrl(string url)
        {
            return await Get<string>(url);
        }


    }
}
