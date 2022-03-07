using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.CertificateModel
{
    public class Size
    {
        public int SizeId { get; set; }
        public double? Thickness { get; set; }
        public double? Width { get; set; }
        public string? Length { get; set; }
    }
}
