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
            //Database.Migrate();
        }

        public MetalContext(DbContextOptions<MetalContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.Migrate();
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
            var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            connectionUrl = connectionUrl.Replace("postgres://", string.Empty);
            var userPassSide = connectionUrl.Split("@")[0];
            var hostSide = connectionUrl.Split("@")[1];

            var user = userPassSide.Split(":")[0];
            var password = userPassSide.Split(":")[1];
            var host = hostSide.Split("/")[0];
            var database = hostSide.Split("/")[1].Split("?")[0];

            var defaultConnectionString = $"Host={host};Database={database};Username={user};Password={password};SSL Mode=Require;Trust Server Certificate=true";

            optionsBuilder.UseNpgsql(defaultConnectionString);

            //optionsBuilder.UseNpgsql("Host=localhost;Database=MetalManagment;Username=postgres;Password=admin");

            //Database.EnsureDeleted();
            //Database.Migrate();
        }
    }
}
