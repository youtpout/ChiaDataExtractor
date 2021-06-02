using OpenSSL.X509Certificate2Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChiaDataExtractor
{
    public class ChiaService
    {
        HttpClient httpClient;
        public ChiaService()
        {
            var certificateFile = ConfigurationManager.AppSettings["cert"];
            var keyFile = ConfigurationManager.AppSettings["key"];

            string cert = File.ReadAllText(certificateFile);
            string key = File.ReadAllText(keyFile);

            ICertificateProvider provider = new CertificateFromFileProvider(cert, key);

            var publicCertificate = X509Certificate2.CreateFromPemFile(certificateFile, keyFile);

            var tmpCert = new X509Certificate2(publicCertificate.Export(X509ContentType.Pfx));

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;
            handler.ClientCertificates.Add(tmpCert);
            httpClient = new HttpClient(handler);
        }


        public async Task<RootBlockchainState> GetBlockchainState()
        {
            RootBlockchainState result = new RootBlockchainState();
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:8555/get_blockchain_state"))
            {
                request.Content = new StringContent("{}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<RootBlockchainState>();
                }
            }

            return result;
        }

        public async Task<string> GetFarming()
        {
            string result = string.Empty;
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:8559/get_reward_targets"))
            {
                request.Content = new StringContent("{}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }

            return result;
        }

        public async Task<string> GetNetworkInfo()
        {
            string result = string.Empty;
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:8555/get_network_info"))
            {
                request.Content = new StringContent("{}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }

            return result;
        }

        public async Task<string> Get(string url)
        {
            string result = string.Empty;
            using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
            {
                request.Content = new StringContent("{}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }

            return result;
        }
    }
}
