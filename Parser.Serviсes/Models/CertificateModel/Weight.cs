using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.CertificateModel
{
    public class Weight
    {
        public int WeightId { get; set; }
        public double? Gross { get; set; }
        public double? Gross2 { get; set; }
        public double? Net { get; set; }
    }
}
