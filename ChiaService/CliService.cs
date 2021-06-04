using ChiaModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public class CliService : ICliService
    {
        string folder = string.Empty;
        string file = string.Empty;
        public CliService(IConfiguration config)
        {
            folder = config["folder"];
            file = config["file"];
        }

        public BlockchainInfo GetBlockChainInfo()
        {
            List<string> result = GetChiaInfos("show -s");
            BlockchainInfo info = new BlockchainInfo();
            if (result.Count > 0)
            {
                foreach (var line in result)
                {
                    if (line.Contains("Current Blockchain Status:"))
                    {

                    }
                }
            }

            return info;
        }

        public string GetFarmingInfo()
        {
            return GetChiaInfos("farm summary").ToString();
        }

        public string GetWalletInfo()
        {
            return GetChiaInfos("wallet show").ToString();
        }

        private List<string> GetChiaInfos(string args)
        {

            List<string> output = new List<string>();

            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(folder, file),
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };


            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                output.Add(line);
            }

            string error = process.StandardError.ReadToEnd();
            Debug.WriteLine(error);

            process.WaitForExit();


            return output;
        }
    }
}
