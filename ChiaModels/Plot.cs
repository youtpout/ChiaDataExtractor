using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChiaModels
{
    public class Plot
    {
        public decimal file_size { get; set; }
        public string filename { get; set; }

        [JsonPropertyName("plot-seed")]
        public string PlotSeed { get; set; }
        public string plot_public_key { get; set; }
        public object pool_contract_puzzle_hash { get; set; }
        public string pool_public_key { get; set; }
        public int size { get; set; }
        public double time_modified { get; set; }
    }

    public class RootPlot
    {
        public List<object> failed_to_open_filenames { get; set; }
        public List<object> not_found_filenames { get; set; }
        public List<Plot> plots { get; set; }
        public bool success { get; set; }
    }

}
