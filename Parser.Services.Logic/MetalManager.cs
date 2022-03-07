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
        private IMetalAccessManager _access;

        public MetalManager(IMetalAccessManager access)
            => _access = access;

        public async Task<int> CreateCertificateAsync(CertificateLink link)
        {
            var uri = new Uri(link.Link);

            Handler nlmkHandler = new NlmkHandler();
            Handler metinvestHandler = new MetinvestHandler();
            Handler severstalHandler = new SeverstalHandler();
            nlmkHandler.Successor = metinvestHandler;
            metinvestHandler.Successor = severstalHandler;

            var certificate = await nlmkHandler.HandleRequestAsync(uri);

            var id = await _access.AddCertificateAsync(certificate);

            return id;
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

        private PackageViewModel MapPackege(Package package)
            => new PackageViewModel()
            {
                SupplyDate = package.DateAdded,
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
