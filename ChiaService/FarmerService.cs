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
    public class FarmerService : IFarmerService
    {
        private readonly IRpcService rpcService;
        private readonly ILogger<FarmerService> _logger;

        public FarmerService(IRpcService rpcService, IConfiguration config, ILogger<FarmerService> logger)
        {
            _logger = logger;
            this.rpcService = rpcService;
        }

        public async Task<FarmingInfo> GetFarmingInfo()
        {
            FarmingInfo infos = new FarmingInfo();
            var blockchainState = await rpcService.GetBlockchainState();
            var farmedAmount = await rpcService.GetFarmedAmount();
            var connections = await rpcService.GetFarmerConnection();
            var plots = await rpcService.GetPlots();

            infos.Synced = blockchainState.blockchain_state.sync.synced;
            infos.Syncing = blockchainState.blockchain_state.sync.sync_mode;
            infos.Running = connections.connections?.Count > 0;
            infos.EstimatedNetworkSpace = blockchainState.blockchain_state.space;
            if (farmedAmount.success)
            {
                infos.PoolRewardAmount = farmedAmount.pool_reward_amount;
                infos.FeeAmount = farmedAmount.fee_amount;
                infos.FarmedAmount = farmedAmount.farmed_amount;
                infos.RewardAmount = farmedAmount.farmer_reward_amount;
                infos.LastHeightFarmed = farmedAmount.last_height_farmed;
            }

            if (plots.success && plots.plots?.Count > 0)
            {
                infos.PlotCount = plots.plots.Count;
                infos.TotalPlotSize = plots.plots.Sum(p => p.file_size);
                if (blockchainState.success)
                {
                    infos.ExpectedTimeToWin = await GetExpectedTimeToWin(plots.plots, blockchainState.blockchain_state);
                }

            }

            return infos;
        }

        private async Task<int> GetExpectedTimeToWin(List<Plot> plots, BlockchainState blockchain)
        {
            int minutes = 0;

            decimal plotSize = plots.Sum(p => p.file_size);
            decimal proportion = plotSize / blockchain.space;

            var avg = await GetAverageBlockTime(blockchain);

            minutes = (int)((avg / 60) / proportion);
            return minutes;
        }

        private async Task<decimal> GetAverageBlockTime(BlockchainState blockchain)
        {
            long blocksToCompare = 500;
            long secondsPerBlock = (24 * 3600) / 4608;

            try
            {
                if (blockchain.peak?.height >= (blocksToCompare + 100))
                {
                    var block = blockchain.peak;
                    while (block != null && block.height > 0 && !block.is_transaction_block)
                    {
                        var result = await rpcService.GetBlockRecord(block.prev_hash);
                        block = result?.block_record;
                    }
                    if (block != null)
                    {
                        var height = block.height - blocksToCompare;
                        var result = await rpcService.GetBlockRecordByHeight(height);
                        var prev = result?.block_record;

                        while (prev != null && prev.height > 0 && !prev.is_transaction_block)
                        {
                            result = await rpcService.GetBlockRecord(prev.prev_hash);
                            prev = result?.block_record;
                        }

                        if (prev != null)
                        {
                            return ((block.timestamp - prev.timestamp) / (block.height - prev.height)).Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }


            return secondsPerBlock;
        }

    }
}
