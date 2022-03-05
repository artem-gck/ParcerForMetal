using Parser.DataAccess;
using Parser.Services.Logic.ChainOfHosts;
using Parser.Serviсes;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;

namespace Parser.Services.Logic
{
    public class ParserManager : IParserManager
    {
        private IDataAccessManager _access;

        public ParserManager(IDataAccessManager access)
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

        public async Task<Certificate> GetCertificateAsync(int id)
            => await _access.GetCertificateAsync(id);
    }
}
