using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChiaService
{
    public interface ICacheService
    {
        Task<X509Certificate2> GetCertificate();
    }
}
