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

        public async Task<Certificate> CheckSertificateAsync(CertificateLink link)
        {
            var certificate = await _access.CheckSertificateAsync(link.Link);

            return certificate;
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
            => await _access.GetAllCertificatesAsync();

        public async Task<List<PackageViewModel>> GetAllPackagesAsync()
        {
            var packeges = await _access.GetAllPackegesAsync();

            return packeges.Select(pac => MapPackege(pac)).ToList();
        }

        public async Task<ExtendedPackageViewModel> GetPackage(int packageId)
        {
            var package = await _access.GetPackage(packageId);
            return MapExtendedPackageViewModel(package);
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

        public async Task UpdateStatusPackageAsync(string batch, int statusId)
            => await _access.UpdateStatusPackageAsync(batch, statusId);

        public async Task<int> AddDeffectToPackage(Defect defect)
        {
            return await _access.AddDeffectToPackage(defect);
        }

        public async Task<int> UpdatePackageAsync(PackageViewModel package)
        {
            return await _access.UpdatePackageAsync(MapPackegeView(package));
        }

        public async Task<int> AddPackageAsync(PackageViewModel package)
        {
            var numberofCert = package.NumberOfCertificate;
            var pac = MapPackegeView(package);

            return await _access.AddPackageAsync(numberofCert, pac);
        }

        public async Task<List<string>> GetNumbersOfCertificates()
        {
            return await _access.GetNumbersOfCertificates();
        }

        private ExtendedPackageViewModel MapExtendedPackageViewModel(Package package)
            => new ()
            {
                PackageId = package.PackageId,

                NumberOfCertificate = package.Certificate.Number,
                Batch = package.Batch,
                SupplyDate = package.DateAdded,
                Supplier = package.Certificate.Author,
                Grade = package.Grade,
                Width = package.Size.Width,
                Thickness = package.Size.Thickness,
                Gros = package.Weight.Gross,
                Net = package.Weight.Net,
                CoatingClass = package.SurfaceQuality,
                Elongation = package.Elongation,
                Sort = package.Variety,
                Price = package.Price,

                NumberOfHeat = package.Heat,
                C = package.ChemicalComposition.C,
                Si = package.ChemicalComposition.Si,
                Mn = package.ChemicalComposition.Mn,
                S = package.ChemicalComposition.S,
                P = package.ChemicalComposition.P,
                Al = package.ChemicalComposition.Al,
                Cr = package.ChemicalComposition.Cr,
                Ni = package.ChemicalComposition.Ni,
                Cu = package.ChemicalComposition.Cu,
                Ti = package.ChemicalComposition.Ti,
                N2 = package.ChemicalComposition.N2,
                As = package.ChemicalComposition.As,

                TrimOfEdge = package.TrimOfEdge,
                TemporalResistance = package.TemporalResistance,
                TensilePoint = package.TensilePoint,
                GrainSize = package.GrainSize,
            };

        private Package MapPackegeView(PackageViewModel package)
        {
            var cert = new Certificate()
            {
                Number = package.NumberOfCertificate,
                Author = package.Supplier,
            };

            var size = new Size()
            {
                Width = package.Width,
                Thickness = package.Thickness,
            };

            var weight = new Weight()
            {
                Net = package.Net,
                Gross = package.Weight,
            };

            var pac = new Package()
            {
                PackageId = package.PackageId,
                DateAdded = package.SupplyDate,
                Batch = package.Batch,
                Grade = package.Grade,
                Certificate = cert,
                Size = size,
                Weight = weight,
                Elongation = package.Elongation,
                Price = package.Price,
                Comment = package.Comment,
                Photo = package.Photo
            };

            return pac;
        }

        private PackageViewModel MapPackege(Package package)
            => new()
            {
                PackageId = package.PackageId,
                SupplyDate = package.DateAdded,
                Batch = package.Batch,
                Grade = package.Grade,
                NumberOfCertificate = package.Certificate.Number,
                Width = package.Size.Width,
                Thickness = package.Size.Thickness,
                Weight = package.Weight.Net,
                Mill = null,
                CoatingClass = package.Batch,
                Sort = package.Variety,
                Supplier = package.Certificate.Author,
                Elongation = package.Elongation,
                Price = package.Price,
                Comment = package.Comment,
                Status = package.Status.StatusName,
                Photo = package.Photo,
                Net = package.Weight.Net,
            };
    }
}
