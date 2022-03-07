using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.CertificateModel
{
    public class Status
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        [JsonIgnore]
        public List<Package> Packages { get; set; } = new List<Package>();
    }
}
