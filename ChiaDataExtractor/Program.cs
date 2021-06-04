using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using Websocket.Client;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Net.Http.Headers;
using System.Collections.Generic;
using OpenSSL.X509Certificate2Provider;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
