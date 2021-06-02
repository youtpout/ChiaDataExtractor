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

            ChiaService service = new ChiaService();

            rpcCall();

            //    GetChiaInfos(folder, "show -s");
            // GetChiaInfos(folder, "farm summary");
            // GetChiaInfos(folder, "wallet show");
            Console.ReadLine();
        }


        async static void rpcCall()
        {
            try
            {

                var url = new Uri("wss://localhost:55400");
                var exitEvent = new ManualResetEvent(false);



                string certificateFile = @"C:\Users\Eddy\.chia\mainnet\config\ssl\full_node\private_full_node.crt";
                string keyFile = @"C:\Users\Eddy\.chia\mainnet\config\ssl\full_node\private_full_node.key";

                string cert = File.ReadAllText(certificateFile);
                string key = File.ReadAllText(keyFile);

                ICertificateProvider provider = new CertificateFromFileProvider(cert, key);

                var publicCertificate = X509Certificate2.CreateFromPemFile(certificateFile, keyFile);

                using (var tmpCert = new X509Certificate2(publicCertificate.Export(X509ContentType.Pfx)))
                {
                    var col = new X509Certificate2Collection();
                    col.Add(tmpCert);
                    var factory = new Func<ClientWebSocket>(() => new ClientWebSocket
                    {
                        Options =
                    {
                        KeepAliveInterval = TimeSpan.FromSeconds(5),
                        ClientCertificates = col,
                        RemoteCertificateValidationCallback= (requestMessage, certificate, chain, policyErrors) => true
                    }
                    });
                    using (var client = new WebsocketClient(url, factory))
                    {
                        client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                        client.ReconnectionHappened.Subscribe(info =>
                            Console.WriteLine($"Reconnection happened, type: {info.Type}"));

                        client.MessageReceived.Subscribe(msg => Console.WriteLine($"Message received: {msg}"));
                        await client.Start();
                        CommandChia cmd = new CommandChia() { ack = false, command = "get_status", service= "chia", data = new Data() { value = "pong" }, request_id = "123456", destination = "wallet", origin = "ui" };
                        string jsonString = JsonSerializer.Serialize(cmd);
                        await Task.Run(() => client.Send(jsonString));

                        exitEvent.WaitOne();
                    }




                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.StackTrace);
            }
        }

        static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static void GetChiaInfos(string folder, string args)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(folder, "chia.exe"),
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.GetEncoding(1252),
                        RedirectStandardError = true,
                        StandardErrorEncoding = Encoding.GetEncoding(1252),
                        CreateNoWindow = true
                    }
                };

                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(line);
                }


                while (!process.StandardError.EndOfStream)
                {
                    var line = process.StandardError.ReadToEnd();
                    Console.WriteLine(line);
                }

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

    public class CommandChia
    {
        public string command { get; set; }
        public bool ack { get; set; }
        public Data data { get; set; }
        public string request_id { get; set; }
        public string destination { get; set; }
        public string origin { get; set; }
        public string service { get; set; }
    }

    public class Data
    {
        public string value { get; set; }
    }


}
