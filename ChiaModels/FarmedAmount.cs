using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
    public class FarmedAmount
    {
        public int farmed_amount { get; set; }
        public int farmer_reward_amount { get; set; }
        public int fee_amount { get; set; }
        public int last_height_farmed { get; set; }
        public int pool_reward_amount { get; set; }
        public bool success { get; set; }
    }
}
