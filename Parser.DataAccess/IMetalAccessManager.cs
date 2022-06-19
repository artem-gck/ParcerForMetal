using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Serviсes.ViewModel;

namespace Parser.DataAccess
{
    public interface IMetalAccessManager
    {
        public Task<int> AddCertificateAsync(Certificate certificate);
        public Task<Certificate> CheckSertificateAsync(string link);
        public Task<Certificate> GetCertificateAsync(int id);
        public Task<Certificate> GetCertificateAsync(string number);
        public Task<List<Certificate>> GetAllCertificatesAsync();
        public Task<int> UpdateSertificateAsync(Certificate certificate);
        public Task<List<Package>> GetAllPackegesAsync();
        public Task UpdateStatusPackageAsync(string batch, int statusId);
        public Task<List<Package>> GetPackagesByStatus(string statusName);
        public Task<int> UpdatePackageAsync(Package package);
        public Task<int> AddPackage(Package package);
        public Task<int> AddPackageAsync(string numberOfSert, Package package);
        public Task<int> AddDeffectToPackage(Defect defect);
        public Task<Package> GetPackage(int packageId);
        public Task<List<string>> GetNumbersOfCertificates();
    }
}
