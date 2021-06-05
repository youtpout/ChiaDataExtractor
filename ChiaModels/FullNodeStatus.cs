using System;

namespace ChiaModels
{
    public class FullNodeStatus
    {
        public string Status { get; set; }
        public string NetworkName { get; set; }
        public string PeakTime { get; set; }
        public string VDF { get; set; }
        public string EstimatedNetworkSpace { get; set; }
        public string ConnectionStatus { get; set; }
        public int PeakHeight { get; set; }
        public int Difficulty { get; set; }
        public long TotalIterations { get; set; }
    }

}
