using System;

namespace ChiaModels
{
    public partial class FullNodeStatus
    {
        public bool Syncing { get; set; }
        public int SyncProgressHeight { get; set; }
        public int SyncTipHeight { get; set; }
        public bool Synced { get; set; }
        public string NetworkName { get; set; }
        public long PeakTime { get; set; }
        public string VDF { get; set; }
        public decimal EstimatedNetworkSpace { get; set; }
        public string ConnectionStatus { get; set; }
        public int PeakHeight { get; set; }
        public int Difficulty { get; set; }
        public long TotalIterations { get; set; }
    }

}
