using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Serviсes.Models.Certificate
{
    public class ImpactStrength
    {
        public int ImpactStrengthId { get; set; }
        public double? KCU { get; set; }
        public double? KCU1 { get; set; }
        public double? KCV { get; set; }
        public double? KCV1 { get; set; }
        public double? AfterMechAgeing { get; set; }
        public double? AfterMechAgeing1 { get; set; }
    }
}
