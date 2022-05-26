using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes.ViewModel
{
    public class Defect
    {
        public string? Batch { get; set; }
        public string? Comment { get; set; }
        public List<byte>? Photo { get; set; }
    }
}
