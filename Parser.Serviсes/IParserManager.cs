using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Certificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes
{
    public interface IParserManager
    {
        public Task<int> CreateCertificateAsync(CertificateLink link);
        public Task<Certificate> GetCertificateAsync(int id);
    }
}
