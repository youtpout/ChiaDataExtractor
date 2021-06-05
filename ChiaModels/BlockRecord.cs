using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class BlockRecord
    {
        public string challenge_block_info_hash { get; set; }
        public ChallengeVdfOutput challenge_vdf_output { get; set; }
        public int deficit { get; set; }
        public string farmer_puzzle_hash { get; set; }
        public object fees { get; set; }
        public object finished_challenge_slot_hashes { get; set; }
        public object finished_infused_challenge_slot_hashes { get; set; }
        public object finished_reward_slot_hashes { get; set; }
        public string header_hash { get; set; }
        public int height { get; set; }
        public InfusedChallengeVdfOutput infused_challenge_vdf_output { get; set; }
        public bool overflow { get; set; }
        public string pool_puzzle_hash { get; set; }
        public string prev_hash { get; set; }
        public object prev_transaction_block_hash { get; set; }
        public int prev_transaction_block_height { get; set; }
        public int required_iters { get; set; }
        public object reward_claims_incorporated { get; set; }
        public string reward_infusion_new_challenge { get; set; }
        public int signage_point_index { get; set; }
        public object sub_epoch_summary_included { get; set; }
        public int sub_slot_iters { get; set; }
        public long? timestamp { get; set; }
        public long total_iters { get; set; }
        public int weight { get; set; }
    }

    public class RootBlockRecord
    {
        public BlockRecord block_record { get; set; }
        public bool success { get; set; }
    }


}
