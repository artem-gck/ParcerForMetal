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
        public Task<Certificate> CreateFromLinkAsync(CertificateLink link);
        public Task<Certificate> CheckSertificateAsync(CertificateLink link);
        public Task<int> CreateCertificateAsync(Certificate certificate);
        public Task<Certificate> GetCertificateAsync(int id);
        public Task<int> UpdateCertificateAsync(Certificate certificate);
        public Task<List<Certificate>> GetAllCertificatesAsync();
        public Task<List<PackageViewModel>> GetAllPackagesAsync();
        public Task UpdateStatusPackageAsync(string batch, int statusId);
        public Task<List<PackageViewModel>> GetPackagesByStatus(string statusName);
        public Task<int> AddPackage(Package package);
        public Task<int> AddDeffectToPackage(Defect defect);
        public Task<ExtendedPackageViewModel> GetPackage(int packageId);
        public Task<int> UpdatePackageAsync(PackageViewModel package);
        public Task<int> AddPackageAsync(PackageViewModel package);
        public Task<List<string>> GetNumbersOfCertificates();
    }
}
