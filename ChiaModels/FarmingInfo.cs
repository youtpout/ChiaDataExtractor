using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
  public partial  class FarmingInfo
    {
        public bool Syncing { get; set; }
        public bool Synced { get; set; }
        public bool Running { get; set; }
        public int FarmedAmount { get; set; }
        public int FeeAmount { get; set; }
        public int RewardAmount { get; set; }
        public int PoolRewardAmount { get; set; }
        public int LastHeightFarmed { get; set; }
        public decimal TotalPlotSize { get; set; }
        public decimal PlotCount { get; set; }
        public decimal EstimatedNetworkSpace { get; set; }
        public int ExpectedTimeToWin { get; set; }


    }
}
