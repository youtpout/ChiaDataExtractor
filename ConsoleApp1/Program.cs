using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            var service = new CliService();


            string show = service.GetBlockChainInfo();
            Console.WriteLine(show);
            string farm = service.GetFarmingInfo();
            Console.WriteLine(farm);
            string wallet = service.GetWalletInfo();
            Console.WriteLine(wallet);

            Console.ReadLine();
        }
    }

}
