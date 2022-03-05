using Microsoft.EntityFrameworkCore;
using Parser.Serviсes.Models.CertificateModel;

namespace Parser.Serviсes.Models.Context
{
    public class MetalContext : DbContext
    {
        public MetalContext()
        {
 
        }

        public MetalContext(DbContextOptions<MetalContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Certificate> Certificate { get; set; } = null!;
        public virtual DbSet<ChemicalComposition> ChemicalComposition { get; set; } = null!;
        public virtual DbSet<ImpactStrength> ImpactStrengths { get; set; } = null!;
        public virtual DbSet<Package> Package { get; set; } = null!;
        public virtual DbSet<Product> Product { get; set; } = null!;
        public virtual DbSet<Size> Size { get; set; } = null!;
        public virtual DbSet<Weight> Weight { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MetalManagment;Integrated Security=True;");
        }
    }
}
