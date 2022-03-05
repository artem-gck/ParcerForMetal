using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parser.Serviсes.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChemicalComposition",
                columns: table => new
                {
                    ChemicalCompositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C = table.Column<double>(type: "float", nullable: true),
                    Mn = table.Column<double>(type: "float", nullable: true),
                    Si = table.Column<double>(type: "float", nullable: true),
                    S = table.Column<double>(type: "float", nullable: true),
                    P = table.Column<double>(type: "float", nullable: true),
                    Cr = table.Column<double>(type: "float", nullable: true),
                    Ni = table.Column<double>(type: "float", nullable: true),
                    Cu = table.Column<double>(type: "float", nullable: true),
                    As = table.Column<double>(type: "float", nullable: true),
                    N2 = table.Column<double>(type: "float", nullable: true),
                    Al = table.Column<double>(type: "float", nullable: true),
                    Ti = table.Column<double>(type: "float", nullable: true),
                    Mo = table.Column<double>(type: "float", nullable: true),
                    W = table.Column<double>(type: "float", nullable: true),
                    V = table.Column<double>(type: "float", nullable: true),
                    AlWithN2 = table.Column<double>(type: "float", nullable: true),
                    Cev = table.Column<double>(type: "float", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChemicalComposition", x => x.ChemicalCompositionId);
                });

            migrationBuilder.CreateTable(
                name: "ImpactStrengths",
                columns: table => new
                {
                    ImpactStrengthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KCU = table.Column<double>(type: "float", nullable: true),
                    KCU1 = table.Column<double>(type: "float", nullable: true),
                    KCV = table.Column<double>(type: "float", nullable: true),
                    KCV1 = table.Column<double>(type: "float", nullable: true),
                    AfterMechAgeing = table.Column<double>(type: "float", nullable: true),
                    AfterMechAgeing1 = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImpactStrengths", x => x.ImpactStrengthId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Labeling = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thickness = table.Column<double>(type: "float", nullable: true),
                    Width = table.Column<double>(type: "float", nullable: true),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeId);
                });

            migrationBuilder.CreateTable(
                name: "Weight",
                columns: table => new
                {
                    WeightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gross = table.Column<double>(type: "float", nullable: true),
                    Gross2 = table.Column<double>(type: "float", nullable: true),
                    Net = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight", x => x.WeightId);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    CertificateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecipientCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contract = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ShipmentShop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WagonNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfRollingStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfPackaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gosts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.CertificateId);
                    table.ForeignKey(
                        name: "FK_Certificate_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    PackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamberConsignmentPackage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Heat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderPosition = table.Column<int>(type: "int", nullable: true),
                    NumberOfClientMaterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrengthGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Variety = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeightId = table.Column<int>(type: "int", nullable: true),
                    CustomerItemNumber = table.Column<int>(type: "int", nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupCode = table.Column<int>(type: "int", nullable: true),
                    PattemCutting = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurfaceQuality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RollingAccuracy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryOfDrawing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOfMatirial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roughness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flatness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrimOfEdge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weldability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderFeatures = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChemicalCompositionId = table.Column<int>(type: "int", nullable: true),
                    SampleLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectOfTestPicses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemporalResistance = table.Column<double>(type: "float", nullable: true),
                    YieldPoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TensilePoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Elongation = table.Column<double>(type: "float", nullable: true),
                    Bend = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hardness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rockwell = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brinel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Eriksen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpactStrengthId = table.Column<int>(type: "int", nullable: true),
                    GrainSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Decarburiization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cementite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Corrosion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestingMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitTemporaryResistance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitYieldStrength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SphericalHoleDepth = table.Column<double>(type: "float", nullable: true),
                    MicroBallCem = table.Column<double>(type: "float", nullable: true),
                    R90 = table.Column<double>(type: "float", nullable: true),
                    N90 = table.Column<double>(type: "float", nullable: true),
                    KoafNavodorag = table.Column<double>(type: "float", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.PackageId);
                    table.ForeignKey(
                        name: "FK_Package_Certificate_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificate",
                        principalColumn: "CertificateId");
                    table.ForeignKey(
                        name: "FK_Package_ChemicalComposition_ChemicalCompositionId",
                        column: x => x.ChemicalCompositionId,
                        principalTable: "ChemicalComposition",
                        principalColumn: "ChemicalCompositionId");
                    table.ForeignKey(
                        name: "FK_Package_ImpactStrengths_ImpactStrengthId",
                        column: x => x.ImpactStrengthId,
                        principalTable: "ImpactStrengths",
                        principalColumn: "ImpactStrengthId");
                    table.ForeignKey(
                        name: "FK_Package_Size_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Size",
                        principalColumn: "SizeId");
                    table.ForeignKey(
                        name: "FK_Package_Weight_WeightId",
                        column: x => x.WeightId,
                        principalTable: "Weight",
                        principalColumn: "WeightId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_ProductId",
                table: "Certificate",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_CertificateId",
                table: "Package",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ChemicalCompositionId",
                table: "Package",
                column: "ChemicalCompositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ImpactStrengthId",
                table: "Package",
                column: "ImpactStrengthId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_SizeId",
                table: "Package",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_WeightId",
                table: "Package",
                column: "WeightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "ChemicalComposition");

            migrationBuilder.DropTable(
                name: "ImpactStrengths");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "Weight");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
