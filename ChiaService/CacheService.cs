using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ChiaService
{
    public class CacheService : ICacheService
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache cache;

        const string certificateKey = "certificateKey";

        public CacheService(IMemoryCache cache, IConfiguration configuration)
        {
            this.cache = cache;
            this.configuration = configuration;
        }

        public async Task<X509Certificate2> GetCertificate()
        {
            var cacheEntry = cache.GetOrCreate(certificateKey, async entry =>
            {
                var certificateFile = configuration["cert"];
                var keyFile = configuration["key"];

                string cert = await File.ReadAllTextAsync(certificateFile);
                string key = await File.ReadAllTextAsync(keyFile);

                var publicCertificate = X509Certificate2.CreateFromPemFile(certificateFile, keyFile);

                return new X509Certificate2(publicCertificate.Export(X509ContentType.Pfx));
            });
            return await cacheEntry;
        }
    }
}
