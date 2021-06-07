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
    public class FullNodeService : IFullNodeService
    {
        private readonly IRpcService rpcService;
        private readonly ILogger<FullNodeService> _logger;

        public FullNodeService(IRpcService rpcService, IConfiguration config, ILogger<FullNodeService> logger)
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
