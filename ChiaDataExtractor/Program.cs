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
            var folder = ConfigurationManager.AppSettings["folder"];



            GetChiaInfos(folder, "show -s");
            GetChiaInfos(folder, "farm summary");
            GetChiaInfos(folder, "wallet show");
            Console.ReadLine();
        }

        static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private async static void GetChiaInfos(string folder, string args)
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(folder, "chia.exe"),
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Encoding.GetEncoding(1252),
                    RedirectStandardError = true,
                    StandardErrorEncoding = Encoding.GetEncoding(1252),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };


                process.Start();
                var output = await process.StandardOutput.ReadToEndAsync();
                Console.WriteLine(output);

                output = await process.StandardError.ReadToEndAsync();
                Console.WriteLine(output);


                //byte[] result = ReadFully(process.StandardOutput.BaseStream);

                //foreach (System.Text.EncodingInfo encodingInfo in System.Text.Encoding.GetEncodings())
                //{
                //    System.Text.Encoding encoding = encodingInfo.GetEncoding();
                //    string decodedBytes = encoding.GetString(result);
                //    System.Console.Out.WriteLine("Encoding: {0}, Decoded Bytes: {1}", encoding.EncodingName, decodedBytes);
                //}



                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

}
