using ChiaModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public class ChiaInfoService : IChiaInfoService
    {
        private readonly IRpcService rpcService;
        private readonly ILogger<ChiaInfoService> _logger;

        public ChiaInfoService(IRpcService rpcService, IConfiguration config, ILogger<ChiaInfoService> logger)
        {
            _logger = logger;
            this.rpcService = rpcService;
        }

        public async Task<FullNodeStatus> GetFullNodeStatus()
        {
            var blockchainState = await rpcService.GetBlockchainState();
            var nodeStatus = new FullNodeStatus();
            if (blockchainState.success)
            {
                var state = blockchainState.blockchain_state;
                nodeStatus.Synced = state.sync.synced;
                nodeStatus.Syncing = state.sync.sync_mode;
                nodeStatus.SyncProgressHeight = state.sync.sync_progress_height;
                nodeStatus.SyncTipHeight = state.sync.sync_tip_height;

                try
                {
                    var networkInfo = await rpcService.GetNetworkInfo();
                    nodeStatus.NetworkName = networkInfo.network_name;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                //nodeStatus.ConnectionStatus =
                nodeStatus.EstimatedNetworkSpace = state.space;
                nodeStatus.Difficulty = state.difficulty;
                nodeStatus.PeakHeight = state.peak.height;
                if (state.peak.timestamp.HasValue)
                {
                    nodeStatus.PeakTime = state.peak.timestamp.Value;
                }
                else
                {
                    try
                    {
                        long timestamp = await GetDateTimeFromBlock(state.peak.header_hash);
                        nodeStatus.PeakTime = timestamp;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }

                nodeStatus.TotalIterations = state.peak.total_iters;
            }

            return nodeStatus;
        }

        public async Task<WalletInfo> GetWallet()
        {
            WalletInfo infos = new WalletInfo();
            var fingerPrint = await rpcService.GetFingerprint();
            var walletInfo = await rpcService.GetWallets();
            var networkInfo = await rpcService.GetNetworkInfo();
            var syncInfo = await rpcService.GetWalletSyncStatus();
            var heightInfo = await rpcService.GetWalletHeighInfo();

            if (fingerPrint.public_key_fingerprints?.Count > 0)
            {
                infos.Fingerprint = fingerPrint.public_key_fingerprints[0];
            }

            infos.Synced = syncInfo.synced;
            infos.Syncing = syncInfo.syncing;
            infos.Height = heightInfo.height;
            if (walletInfo.success && walletInfo.wallets?.Count > 0)
            {
                WalletBalanceParam param = new WalletBalanceParam() { wallet_id = walletInfo.wallets[0].id };
                var balance = await rpcService.GetWalletBalance(param);
                infos.PendingBalance = balance.wallet_balance.pending_change;
                infos.TotalBalance = balance.wallet_balance.confirmed_wallet_balance;
                infos.SpendableBalance = balance.wallet_balance.spendable_balance;
            }

            return infos;
        }

        private async Task<long> GetDateTimeFromBlock(string hash)
        {
            var blockRecord = await rpcService.GetBlockRecord(hash);
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
    }
}
