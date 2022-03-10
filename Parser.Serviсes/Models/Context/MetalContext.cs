using Microsoft.EntityFrameworkCore;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.Models.Login;
using System.Security.Cryptography;
using System.Text;

namespace Parser.Serviсes.Models.Context
{
    public class MetalContext : DbContext
    {
        public MetalContext()
        {
            //Database.EnsureDeleted();
        }

        public MetalContext(DbContextOptions<MetalContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
        }

        public virtual DbSet<Certificate> Certificate { get; set; } = null!;
        public virtual DbSet<ChemicalComposition> ChemicalComposition { get; set; } = null!;
        public virtual DbSet<ImpactStrength> ImpactStrengths { get; set; } = null!;
        public virtual DbSet<Package> Package { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<Size> Size { get; set; } = null!;
        public virtual DbSet<Weight> Weight { get; set; } = null!;
        public virtual DbSet<Status> Status { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;
        public virtual DbSet<UserInfo> UserInfo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MetalManagment;Integrated Security=True;");
        }
    }
}
