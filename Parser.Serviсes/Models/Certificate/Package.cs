using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcer.Model
{
    public class Package
    {
        public int PackageId { get; set; }
        public string? NamberConsignmentPackage { get; set; }
        public string? Heat { get; set; }
        public string? Batch { get; set; }
        public string? Grade { get; set; }
        public string? Category { get; set; }
        public string? StrengthGroup { get; set; }
        public Size? Size { get; set; }
        public int? Quantity { get; set; }
        public string? Variety { get; set; }
        public string? Gost { get; set; }
        public Weight? Weight { get; set; }
        public int? CustomerItemNumber { get; set; }
        public string? Treatment { get; set; }
        public int? GroupCode { get; set; }
        public string? PattemCutting { get; set; }
        public string? SurfaceQuality { get; set; }
        public string? RollingAccuracy { get; set; }
        public string? CategoryOfDrawing { get; set; }
        public string? StateOfMatirial { get; set; }
        public string? Roughness { get; set; }
        public string? Flatness { get; set; }
        public string? TrimOfEdge { get; set; }
        public string? Weldability { get; set; }
        public string? OrderFeatures { get; set; }
        public ChemicalComposition? ChemicalComposition { get; set; }
        public string? SampleLocation { get; set; }
        public string? DirectOfTestPicses { get; set; }
        public double? TemporalResistance { get; set; }
        public string? YieldPoint { get; set; }
        public double? Elongation { get; set; }
        public string? Bend { get; set; }
        public string? Hardness { get; set; }
        public string? Rockwell { get; set; }
        public string? Brinel { get; set; }
        public string? Eriksen { get; set; }
        public ImpactStrength? ImpactStrength { get; set; }
        public string? GrainSize { get; set; }
        public string? Decarburiization { get; set; }
        public string? Cementite { get; set; }
        public string? Banding { get; set; }
        public string? Corrosion { get; set; }
        public string? TestingMethod { get; set; }
        public string? UnitTemporaryResistance { get; set; }
        public string? UnitYieldStrength { get; set; }
        public double? SphericalHoleDepth { get; set; }
        public double? MicroBallCem { get; set; }
        public double? R90 { get; set; }
        public double? N90 { get; set; }
        public double? KoafNavodorag { get; set; }
        public string? Notes { get; set; }
    }
}
