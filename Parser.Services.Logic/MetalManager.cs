using Newtonsoft.Json;
using Parser.DataAccess;
using Parser.Services.Logic.ChainOfHosts;
using Parser.Serviсes;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.ViewModel;

namespace Parser.Services.Logic
{
    public class MetalManager : IMetalManager
    {
        private readonly IMetalAccessManager _access;

        public MetalManager(IMetalAccessManager access)
            => _access = access;

        public async Task<int> AddPackage(Package package)
        {
            var id = await _access.AddPackage(package);

            return id;
        }

        public async Task<int> CreateCertificateAsync(Certificate certificate)
        {
            var id = await _access.AddCertificateAsync(certificate);

            return id;
        }

        public async Task<Certificate> CreateFromLinkAsync(CertificateLink link)
        {
            var uri = new Uri(link.Link);

            Handler nlmkHandler = new NlmkHandler();
            Handler metinvestHandler = new MetinvestHandler();
            Handler severstalHandler = new SeverstalHandler();
            nlmkHandler.Successor = metinvestHandler;
            metinvestHandler.Successor = severstalHandler;

            var certificate =  await nlmkHandler.HandleRequestAsync(uri);
            var cert = await _access.GetCertificateAsync(certificate.Number);

            if (cert is null)
            {
                var stringCert = JsonConvert.SerializeObject(certificate);
                var certificateDb = JsonConvert.DeserializeObject<Certificate>(stringCert);

                certificateDb.Packages = new List<Package>();
                
                var id = await _access.AddCertificateAsync(certificateDb);

                certificate.CertificateId = id;
                return certificate;
            }
            else
            {
                cert.Packages = certificate.Packages;
                return cert;
            }
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
            => await _access.GetAllCertificatesAsync();

        public async Task<List<PackageViewModel>> GetAllPackagesAsync()
        {
            var packeges = await _access.GetAllPackegesAsync();

            return packeges.Select(pac => MapPackege(pac)).ToList();
        }

        public async Task<Certificate> GetCertificateAsync(int id)
            => await _access.GetCertificateAsync(id);

        public async Task<List<PackageViewModel>> GetPackagesByStatus(string statusName)
        {
            var packages = await _access.GetPackagesByStatus(statusName);
            return packages.Select(pac => MapPackege(pac)).ToList();
        }

        public async Task<int> UpdateCertificateAsync(Certificate certificate)
            => await _access.UpdateSertificateAsync(certificate);

        public async Task UpdateStatusPackageAsync(string batch, string statusName)
            => await _access.UpdateStatusPackageAsync(batch, statusName);

        public async Task<int> AddDeffectToPackage(Defect defect)
        {
            return await _access.AddDeffectToPackage(defect);
        }

        private PackageViewModel MapPackege(Package package)
            => new()
            {
                SupplyDate = package.DateAdded,
                Batch = package.Batch,
                Grade = package.Grade,
                NumberOfCertificate = package.Certificate.Number,
                Width = package.Size.Width,
                Thickness = package.Size.Thickness,
                Weight = package.Weight.Net,
                Mill = null,
                CoatingClass = package.SurfaceQuality,
                Sort = package.Variety,
                Supplier = package.Certificate.Author,
                Elongation = package.Elongation,
                Price = null,
                Comment = package.Comment,
                Status = package.Status.StatusName,
            };
    }
}
