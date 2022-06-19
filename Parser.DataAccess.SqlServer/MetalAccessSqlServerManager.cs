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

        public async Task<Certificate> CheckSertificateAsync(string link)
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
                                                        .FirstOrDefaultAsync(cert => cert.Link == link);

            return certificate;
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

        public async Task UpdateStatusPackageAsync(string batch, int statusId)
        {
            await UpdateStatus();

            var status = await _context.Status.FindAsync(statusId);

            var package = await _context.Package.Include(pac => pac.Status).FirstOrDefaultAsync(pac => pac.Batch == batch);

            package.Status = status;
            package.DateChange = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<int> AddPackage(Package package)
        {
            package.DateAdded = DateTime.UtcNow;
            package.DateChange = DateTime.UtcNow;
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

        public async Task<int> AddPackageAsync(string numberOfCert, Package package)
        {
            package.DateChange = DateTime.Now;
            package.Status = await GetStatus("Имеется");

            var cert = await _context.Certificate.FirstOrDefaultAsync(cert => cert.Number == numberOfCert);
            package.Certificate = cert;

            var id = await _context.Package.AddAsync(package);
            await _context.SaveChangesAsync();
            return id.Entity.PackageId;
        }

        public async Task<Package> GetPackage(int packageId)
        {
            var package = await _context.Package.Include(pac => pac.ChemicalComposition)
                                                .Include(pac => pac.ImpactStrength)
                                                .Include(pac => pac.Size)
                                                .Include(pac => pac.Weight)
                                                .Include(pac => pac.Status)
                                                .Include(pac => pac.Certificate)
                                                .FirstOrDefaultAsync(pac => pac.PackageId == packageId);

            return package;
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

        public async Task<int> UpdatePackageAsync(Package package)
        {
            var packageDb = await _context.Package.Include(pac => pac.ChemicalComposition)
                                     .Include(pac => pac.ImpactStrength)
                                     .Include(pac => pac.Size)
                                     .Include(pac => pac.Weight)
                                     .Include(pac => pac.Status)
                                     .Include(pac => pac.Certificate)
                                     .FirstOrDefaultAsync(pac => pac.PackageId == package.PackageId);

            packageDb.Weight.Net = package.Weight.Net;
            packageDb.Weight.Gross = package.Weight.Gross;
            packageDb.Batch = package.Batch;
            packageDb.Grade = package.Grade;
            packageDb.Certificate.Author = package.Certificate.Author;
            packageDb.Elongation = package.Elongation;
            packageDb.Size.Thickness = package.Size.Thickness;
            packageDb.Size.Width = package.Size.Width;
            packageDb.Price = package.Price;
            packageDb.Comment = package.Comment;

            await _context.SaveChangesAsync();

            return packageDb.PackageId;
            //_package.NumberOfCertificate = tbNumberCertificate.Text;
        }

        public async Task<List<string>> GetNumbersOfCertificates()
        {
            return await _context.Certificate.Select(cert => cert.Number).ToListAsync();
        }

        private async Task UpdateStatus()
        {
            var statusProcessing = await GetStatus("В обработке");
            var statusUses = await GetStatus("Использован");

            //var a = await _context.Package.ToListAsync();

            var packagesDelete = _context.Package.Where(pac => pac.Status == statusUses)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.SpecifyKind(DateTime.Now.AddDays(-UpdateTimeDays), DateTimeKind.Utc));
           
            _context.Package.RemoveRange(packagesDelete);

            var packagesUpdate = _context.Package.Where(pac => pac.Status == statusProcessing)
                                                 .Where(pac => pac.DateChange != null)
                                                 .Where(pac => pac.DateChange <= DateTime.SpecifyKind(DateTime.Now.AddHours(-UpdateTimeHours), DateTimeKind.Utc))
                                                 .ToList();

            for (var i = 0; i < packagesUpdate.Count; i++)
            {
                packagesUpdate[i].Status = statusUses;
                packagesUpdate[i].DateChange = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
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
    }
}
