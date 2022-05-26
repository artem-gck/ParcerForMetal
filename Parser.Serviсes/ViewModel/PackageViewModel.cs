using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes.ViewModel
{
    public class PackageViewModel
    {
        public DateTime? SupplyDate { get; set; }
        public string Batch { get; set; }
        public string? Grade { get; set; }
        public string? NumberOfCertificate { get; set; }
        public double? Width { get; set; }
        public double? Thickness { get; set; }
        public double? Weight { get; set; }
        public string? Mill { get; set; }
        public string? CoatingClass { get; set; }
        public string? Sort { get; set; }
        public string? Supplier { get; set; }
        public double? Elongation { get; set; }
        public decimal? Price { get; set; }
        public string? Comment { get; set; }
        public string? Status { get; set; }
    }
}
