using Newtonsoft.Json;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Certificate;
using Parser.Serviсes.Models.NlmkJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Services.Logic.ChainOfHosts
{
    public class NlmkHandler : Handler
    {
        private readonly string context = "nlmk";

        public override async Task<Certificate> HandleRequestAsync(Uri link)
        {
            if (link.AbsoluteUri.Contains(context))
            {
                return await GetCertificateAsync(link);
            }
            else if (Successor != null)
            {
                return await Successor.HandleRequestAsync(link);
            }

            return null;
        }

        private async Task<Certificate> GetCertificateAsync(Uri link)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.82 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");

            _httpClient.BaseAddress = new Uri("https://doc.nlmk.shop/api/v1/views/certificates/");

            var identyOfCertificate = link.GetComponents(UriComponents.Query, UriFormat.UriEscaped);

            var page = await _httpClient.GetAsync($"{identyOfCertificate[2..]}?lang=ru");
            var bodyOfPage = await page.Content.ReadAsStringAsync();
            var root = JsonConvert.DeserializeObject<Root>(bodyOfPage);

            var certificate = new Certificate();

            certificate.Number = root.Elements[1].Elements[0].Value.ToString()[13..19];
            certificate.Date = DateTime.Parse(root.Elements[1].Elements[0].Value.ToString()[22..33]);
            certificate.WagonNumber = root.Elements[5].Elements[1].Value.ToString();
            certificate.Product = GetProduct(root);
            certificate.ShipmentShop = root.Elements[5].Elements[0].Value.ToString();
            certificate.WagonNumber = root.Elements[5].Elements[1].Value.ToString();
            certificate.OrderNumber = root.Elements[1].Elements[0].Value.ToString()[43..];
            certificate.TypeOfRollingStock = root.Elements[5].Elements[2].Value.ToString();
            certificate.Notes = root.Elements[5].Elements[3].Value.ToString();
            certificate.Packages = new List<Package>();

            for (var i = 0; i < root.Elements[4].Elements[0].Elements[0].Body.Count; i++)
            {
                certificate.Packages.Add(GetPackage(root, i));
            }

            certificate.Link = link.AbsoluteUri;

            return certificate;
        }

        private static Product GetProduct(Root root)
        {
            var product = new Product();

            product.Name = root.Elements[1].Elements[1].Value.ToString();

            return product;
        }

        private static Package GetPackage(Root root, int id)
        {
            var package = new Package();

            package.NamberConsignmentPackage = root.Elements[4].Elements[1].Elements[0].Body[id].Tr[1];
            package.Heat = root.Elements[4].Elements[1].Elements[0].Body[id].Tr[2];
            package.Batch = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[0];
            package.Grade = root.Elements[4].Elements[0].Elements[0].Body[id].Tr[6];
            package.Size = GetSize(root, id);
            package.Quantity = int.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[9]);
            package.Variety = root.Elements[4].Elements[0].Elements[0].Body[id].Tr[3];
            package.Gost = root.Elements[4].Elements[0].Elements[0].Body[id].Tr[14];
            package.Weight = GetWeight(root, id);
            package.SurfaceQuality = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[3];
            package.CategoryOfDrawing = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[6];
            package.TrimOfEdge = root.Elements[4].Elements[2].Elements[0].Body[id].Tr[5];
            package.ChemicalComposition = GetChemicalComposition(root, id);
            package.TemporalResistance = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[8], CultureInfo.InvariantCulture);
            package.Elongation = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[9], CultureInfo.InvariantCulture);
            package.SphericalHoleDepth = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[10], CultureInfo.CurrentCulture);
            package.MicroBallCem = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[15], CultureInfo.InvariantCulture);
            package.R90 = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[17], CultureInfo.InvariantCulture);
            package.N90 = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[18], CultureInfo.InvariantCulture);
            package.KoafNavodorag = double.Parse(root.Elements[4].Elements[2].Elements[0].Body[id].Tr[19], CultureInfo.InvariantCulture);

            return package;
        }

        private static Size GetSize(Root root, int id)
        {
            var size = new Size();

            size.Width = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[8][7..], CultureInfo.InvariantCulture);
            size.Thickness = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[8][..4], CultureInfo.InvariantCulture);

            return size;
        }

        private static Weight GetWeight(Root root, int id)
        {
            var weight = new Weight();

            weight.Gross = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[11], CultureInfo.InvariantCulture);
            weight.Net = double.Parse(root.Elements[4].Elements[0].Elements[0].Body[id].Tr[10], CultureInfo.InvariantCulture);

            return weight;
        }

        private static ChemicalComposition GetChemicalComposition(Root root, int id)
        {
            var chemical = new ChemicalComposition();

            chemical.C = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[3], CultureInfo.InvariantCulture);
            chemical.Si = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[4], CultureInfo.InvariantCulture);
            chemical.Mn = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[5], CultureInfo.InvariantCulture);
            chemical.S = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[6], CultureInfo.InvariantCulture);
            chemical.P = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[7], CultureInfo.InvariantCulture);
            chemical.Al = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[8], CultureInfo.InvariantCulture);
            chemical.Cr = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[9], CultureInfo.InvariantCulture);
            chemical.Ni = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[10], CultureInfo.InvariantCulture);
            chemical.Cu = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[11], CultureInfo.InvariantCulture);
            chemical.Ti = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[12], CultureInfo.InvariantCulture);
            chemical.V = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[14], CultureInfo.InvariantCulture);
            chemical.N2 = double.Parse("0" + root.Elements[4].Elements[1].Elements[0].Body[id].Tr[15], CultureInfo.InvariantCulture);

            return chemical;
        }
    }
}
