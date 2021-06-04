using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class CliService
    {
        string folder = string.Empty;
        string file = string.Empty;
        public CliService()
        {
            folder = ConfigurationManager.AppSettings["folder"];
            file = ConfigurationManager.AppSettings["file"];
        }

        public string GetBlockChainInfo()
        {
            return GetChiaInfos("show -s");
        }

        public string GetFarmingInfo()
        {
            return GetChiaInfos("farm summary");
        }

        public string GetWalletInfo()
        {
            return GetChiaInfos("wallet show");
        }

        private string GetChiaInfos(string args)
        {

            string output = string.Empty;
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(folder, file),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.GetEncoding(1252),
                    RedirectStandardError = true,
                    StandardErrorEncoding = Encoding.GetEncoding(1252),
                    CreateNoWindow = true
                };


                process.Start();
                output = process.StandardOutput.ReadToEnd();

                string error = process.StandardError.ReadToEnd();
                Console.WriteLine(error);

                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return output;
        }
    }
}
