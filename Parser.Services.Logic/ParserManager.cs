﻿using Parser.DataAccess;
using Parser.Services.Logic.ChainOfHosts;
using Parser.Serviсes;
using Parser.Serviсes.Models;

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
            var certificate = await nlmkHandler.HandleRequestAsync(uri);

            var id = await _access.AddCertificateAsync(certificate);

            return id;
        }
    }
}
