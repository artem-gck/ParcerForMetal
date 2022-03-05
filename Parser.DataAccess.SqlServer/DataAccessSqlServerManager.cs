using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.DataAccess.SqlServer
{
    public class DataAccessSqlServerManager : IDataAccessManager
    {
        private MetalContext _context;

        public DataAccessSqlServerManager(MetalContext context)
            => _context = context;

        public async Task<int> AddCertificateAsync(Certificate certificate)
        {
            certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));

            var cert = await _context.Certificate.AddAsync(certificate);
            await _context.SaveChangesAsync();

            return cert.OriginalValues.GetValue<int>("CertificateId");
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
        {
            var certificates = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .ToListAsync();

            return certificates;
        }

        public async Task<Certificate> GetCertificateAsync(int id)
        {
            var certificate = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Where(cert => cert.CertificateId == id).FirstOrDefaultAsync();

            return certificate;
        }
    }
}
