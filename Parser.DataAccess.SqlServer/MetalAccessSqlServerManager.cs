using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Parser.Serviсes.ViewModel;

#pragma warning disable CS8620

namespace Parser.DataAccess.SqlServer
{
    public class MetalAccessSqlServerManager : IMetalAccessManager
    {
        private readonly int UpdateTimeHours;
        private readonly int UpdateTimeDays;

        private MetalContext _context;

        public MetalAccessSqlServerManager(MetalContext context, IConfiguration configuration)
        {
            _context = context;
            UpdateTimeHours = int.Parse(configuration.GetSection("UpdateTimeHours").Value);
            UpdateTimeDays = int.Parse(configuration.GetSection("UpdateTimeDays").Value);
        }

        public async Task<int> AddCertificateAsync(Certificate certificate)
        {
            certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));

            await SetStatus(certificate);

            var cert = await _context.Certificate.AddAsync(certificate);
            await _context.SaveChangesAsync();

            await UpdateStatus();

            return cert.OriginalValues.GetValue<int>("CertificateId");
        }

        public async Task<List<Certificate>> GetAllCertificatesAsync()
        {
            //_context.Database.Migrate();

            await UpdateStatus();

            var certificates = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .ToListAsync();

            return certificates;
        }

        public async Task<Certificate> GetCertificateAsync(int id)
        {
            await UpdateStatus();

            var certificate = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .Where(cert => cert.CertificateId == id).FirstOrDefaultAsync();

            return certificate;
        }

        public async Task<Certificate> GetCertificateAsync(string number)
        {
            await UpdateStatus();

            var certificate = await _context.Certificate.Include(cert => cert.Product)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Size)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Weight)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.ChemicalComposition)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.ImpactStrength)
                .Include(cert => cert.Packages)
                .ThenInclude(pac => pac.Status)
                .Where(cert => cert.Number == number).FirstOrDefaultAsync();

            return certificate;
        }

        public async Task<int> UpdateSertificateAsync(Certificate certificate)
        {
            await UpdateStatus();

            var cert = await _context.Certificate.Include(cert => cert.Product)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Size)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Weight)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ChemicalComposition)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.ImpactStrength)
                                                 .Include(cert => cert.Packages)
                                                     .ThenInclude(pac => pac.Status)
                                                 .Where(cert => cert.CertificateId == certificate.CertificateId).FirstOrDefaultAsync();

            for (var i = 0; i < certificate.Packages.Count(); i++)
            {
                cert.Packages[i].Weight.Net = certificate.Packages[i].Weight.Net;
                cert.Packages[i].Size.Thickness = certificate.Packages[i].Size.Thickness;
            }

            await _context.SaveChangesAsync();

            return certificate.CertificateId;
        }

        public async Task<List<Package>> GetAllPackegesAsync()
        {
            await UpdateStatus();

            var packeges = await _context.Package.Include(pac => pac.Certificate)
                                                 .Include(pac => pac.Weight)
                                                 .Include(pac => pac.Size)
                                                 .Include(pac => pac.Status)
                                                 .ToListAsync();

            return packeges;
        }

        public async Task<List<Package>> GetPackagesByStatus(string statusName)
        {
            await UpdateStatus();

            var status = await GetStatus(statusName);

            var packeges = await _context.Package.Include(pac => pac.Certificate)
                                                 .Include(pac => pac.Weight)
                                                 .Include(pac => pac.Size)
                                                 .Include(pac => pac.Status)
                                                 .Where(pac => pac.Status == status)
                                                 .ToListAsync();

            return packeges;
        }

        public async Task UpdateStatusPackageAsync(string batch, string statusName)
        {
            await UpdateStatus();

            var status = await GetStatus(statusName);

            var package = await _context.Package.Include(pac => pac.Status).FirstOrDefaultAsync(pac => pac.Batch == batch);

            package.Status = status;
            package.DateChange = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateStatus()
        {
            var statusProcessing = await GetStatus("В обработке");
            var statusUses = await GetStatus("Использован");

            var packagesDelete = _context.Package.Where(pac => pac.Status == statusUses)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.Now.AddDays(-UpdateTimeDays));
           
            _context.Package.RemoveRange(packagesDelete);

            var packagesUpdate = _context.Package.Where(pac => pac.Status == statusProcessing)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.Now.AddHours(-UpdateTimeHours))
                                                 .ToList();

            for (var i = 0; i < packagesUpdate.Count; i++)
            {
                packagesUpdate[i].Status = statusUses;
                packagesUpdate[i].DateChange = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

        private async Task SetStatus(Certificate certificate)
        {
            var status = await GetStatus("Имеется");

            for (var i = 0; i < certificate.Packages.Count; i++)
            {
                certificate.Packages[i].DateAdded = DateTime.Now;
                certificate.Packages[i].DateChange = DateTime.Now;
                certificate.Packages[i].Status = status;
            }
        }

        private async Task<Status> GetStatus(string name)
        {
            var status = await _context.Status.Where(status => status.StatusName == name)
                                              .FirstOrDefaultAsync();

            if (status is null)
            {
                status = new Status()
                {
                    StatusName = name,
                };

                await _context.Status.AddAsync(status);
                await _context.SaveChangesAsync();
            }

            return status;
        }

        public async Task<int> AddPackage(Package package)
        {
            package.DateAdded = DateTime.Now;
            package.DateChange = DateTime.Now;
            package.Status = await GetStatus("Имеется");

            var pace = await _context.Package.Include(pac => pac.Weight)
                .Include(pac => pac.Size).FirstOrDefaultAsync(p => p.Batch == package.Batch);

            if (pace is null)
            {
                var pac = await _context.Package.AddAsync(package);
                await _context.SaveChangesAsync();

                await UpdateStatus();

                return pac.OriginalValues.GetValue<int>("PackageId");
            }
            else
            {
                pace.Grade = package.Grade;
                pace.Size.Thickness = package.Size.Thickness;
                pace.Size.Width = package.Size.Width;
                pace.Weight.Gross = package.Weight.Gross;

                await _context.SaveChangesAsync();

                await UpdateStatus();

                return pace.PackageId;
            }
        }

        public async Task<int> AddDeffectToPackage(Defect defect)
        {
            var package = await _context.Package.FirstOrDefaultAsync(pac => pac.Batch == defect.Batch);

            package.Photo = defect.Photo.ToArray();
            package.Comment = defect.Comment;
            package.Status = await GetStatus("С дефектом");

            _context.SaveChangesAsync();

            return package.PackageId;
        }
    }
}
