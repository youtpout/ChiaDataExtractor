using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;

namespace ChiaDataExtractor
{
    class Program
    {
        //  static HttpClient client = new HttpClient();
        //  static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.GetEncoding(1252);

            var host = new WebHostBuilder()
           .UseKestrel()
           .UseStartup<Startup>()
           .Build();

            host.Run();

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
