using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes
{
    public interface IMetalManager
    {
        public Task<int> CreateCertificateAsync(CertificateLink link);
        public Task<Certificate> GetCertificateAsync(int id);
        public Task<List<Certificate>> GetAllCertificatesAsync();
        public Task<List<PackageViewModel>> GetAllPackagesAsync();
    }
}
