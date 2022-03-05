using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Parser.Serviсes.Models.CertificateModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Services.Logic.ChainOfHosts
{
    public class SeverstalHandler : Handler
    {
        private readonly string context = "severstal";

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
            using var _httpClient = new HttpClient();
            var a = await _httpClient.GetAsync(link);
            var b = await a.Content.ReadAsStringAsync();

            var parser = new HtmlParser();
            var document = parser.ParseDocument(b);

            var countOfPackages = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("№ П/П"))
                                 .Last().GetElementsByTagName("tr")
                                        .Count() - 1;

            var certificate = new Certificate();

            certificate.Link = link.AbsoluteUri;
            certificate.Number = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("Сертификат №:")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            certificate.Date = DateTime.Parse(document.QuerySelectorAll("tr").Where(element => element.Text().Contains("Сертификат №:")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[3].Trim());
            certificate.Recipient = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("ГРУЗОПОЛУЧАТЕЛЬ, АДРЕС")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            certificate.SpecificationNumber = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("СПЕЦИФИКАЦИЯ №")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            certificate.RecipientCountry = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("СТРАНА НАЗНАЧЕНИЯ")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            certificate.Gosts = document.QuerySelectorAll("td").Where(element => element.Text().Contains("ГОСТ")).Select(element => element.Text()).Last().Trim();
            certificate.TypeOfPackaging = document.QuerySelectorAll("tbody").Where(element => element.Text().Contains("НАИМЕНОВАНИЕ И КОД ТОВАРА")).Last().Children.Last().Children.First().Children.First().Children.First().Children.First().Text().Trim();
            certificate.Notes = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("Примечания")).Last().GetElementsByTagName("td").Last().Text().Trim();
            certificate.Product = GetProduct(document);

            certificate.Packages = new List<Package>();

            for (var i = 0; i < countOfPackages; i++)
                certificate.Packages.Add(GetPackage(document, i));

            return certificate;
        }

        private static Product GetProduct(IHtmlDocument document)
        {
            var product = new Product();

            var access = document.QuerySelectorAll("tbody").Where(element => element.Text().Contains("НАИМЕНОВАНИЕ И КОД ТОВАРА")).Last().GetElementsByTagName("tr").ToArray()[1].GetElementsByTagName("td").Select(element => element.Text()).First().Trim();

            product.Name = string.IsNullOrWhiteSpace(access) ? null : access;

            return product;
        }

        private static Package GetPackage(IHtmlDocument document, int id)
        {
            var package = new Package();

            var access = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("№ П/П"))
                                 .Last().GetElementsByTagName("tr")
                                        .ToArray()[id + 1].GetElementsByTagName("td")
                                               .Select(element => element.Text())
                                               .ToArray();

            var accessMech = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("Предел текучести"))
                                 .Last().GetElementsByTagName("tr")
                                        .Last().GetElementsByTagName("td")
                                               .Select(element => element.Text())
                                               .ToArray();

            var orderPosition = string.IsNullOrWhiteSpace(access[7]) ? null : access[7];
            var count = string.IsNullOrWhiteSpace(access[29]) ? null : access[29];
            var elongation = string.IsNullOrWhiteSpace(accessMech[3]) ? null : accessMech[3];
            var sphericalHoleDepth = string.IsNullOrWhiteSpace(accessMech[7]) ? null : accessMech[7];


            package.NamberConsignmentPackage = string.IsNullOrWhiteSpace(access[4]) ? null : access[4];
            package.Heat = string.IsNullOrWhiteSpace(access[5]) ? null : access[5];
            package.Batch = string.IsNullOrWhiteSpace(access[6]) ? null : access[6];
            package.OrderPosition = orderPosition is null ? null : int.Parse(orderPosition);
            package.Quantity = count is null ? null : int.Parse(count);
            package.NumberOfClientMaterial = string.IsNullOrWhiteSpace(access[20]) ? null : access[20];
            package.Category = string.IsNullOrWhiteSpace(access[25]) ? null : access[25];
            package.SerialNumber = string.IsNullOrWhiteSpace(access[27]) ? null : access[27];
            package.Grade = string.IsNullOrWhiteSpace(access[28]) ? null : access[28];
            package.StrengthGroup = string.IsNullOrWhiteSpace(access[41]) ? null : access[41];
            package.Profile = string.IsNullOrWhiteSpace(access[30]) ? null : access[30];
            package.Barcode = string.IsNullOrWhiteSpace(access[31]) ? null : access[31];
            package.TrimOfEdge = string.IsNullOrWhiteSpace(access[37]) ? null : access[37];
            package.Flatness = string.IsNullOrWhiteSpace(access[35]) ? null : access[35];
            package.YieldPoint = string.IsNullOrWhiteSpace(accessMech[1]) ? null : accessMech[1];
            package.TensilePoint = string.IsNullOrWhiteSpace(accessMech[2]) ? null : accessMech[2];
            package.Elongation = elongation is null ? null : double.Parse(elongation, CultureInfo.InvariantCulture);
            package.Rockwell = string.IsNullOrWhiteSpace(accessMech[4]) ? null : accessMech[4];
            package.GrainSize = string.IsNullOrWhiteSpace(accessMech[5]) ? null : accessMech[5];
            package.Cementite = string.IsNullOrWhiteSpace(accessMech[6]) ? null : accessMech[6];
            package.SphericalHoleDepth = sphericalHoleDepth is null ? null : double.Parse(sphericalHoleDepth, CultureInfo.InvariantCulture);

            package.ChemicalComposition = GetChemicalComposition(document);
            package.Size = GetSize(document, id);
            package.Weight = GetWeight(document, id);

            return package;
        }

        private static ChemicalComposition GetChemicalComposition(IHtmlDocument document, int id = 0)
        {
            var chemical = new ChemicalComposition();

            var access = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("Химический состав"))
                                 .Last().GetElementsByTagName("tr")
                                        .Last().GetElementsByTagName("td")
                                               .Select(element => element.Text())
                                               .ToArray();

            chemical.C = double.Parse(access[0], CultureInfo.InvariantCulture);
            chemical.Si = double.Parse(access[1], CultureInfo.InvariantCulture);
            chemical.Mn = double.Parse(access[2], CultureInfo.InvariantCulture);
            chemical.S = double.Parse(access[3], CultureInfo.InvariantCulture);
            chemical.P = double.Parse(access[4], CultureInfo.InvariantCulture);
            chemical.Al = double.Parse(access[5], CultureInfo.InvariantCulture);

            return chemical;
        }

        private static Size GetSize(IHtmlDocument document, int id)
        {
            var size = new Size();

            var access = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("№ П/П"))
                                 .Last().GetElementsByTagName("tr")
                                        .ToArray()[id + 1].GetElementsByTagName("td")
                                               .Select(element => element.Text())
                                               .ToArray();

            size.Width = string.IsNullOrWhiteSpace(access[21]) ? null : double.Parse(access[21][8..], CultureInfo.InvariantCulture);
            size.Thickness = string.IsNullOrWhiteSpace(access[21]) ? null : double.Parse(access[21][..5], CultureInfo.InvariantCulture);
            size.Length = string.IsNullOrWhiteSpace(access[11]) ? null : access[11];

            return size;
        }

        private static Weight GetWeight(IHtmlDocument document, int id)
        {
            var weight = new Weight();

            var access = document.QuerySelectorAll("tbody")
                                 .Where(element => element.Text().Contains("№ П/П"))
                                 .Last().GetElementsByTagName("tr")
                                        .ToArray()[id + 1].GetElementsByTagName("td")
                                            .Select(element => element.Text())
                                            .ToArray();

            weight.Gross = string.IsNullOrWhiteSpace(access[13]) ? null : double.Parse(access[13], CultureInfo.InvariantCulture);
            weight.Net = string.IsNullOrWhiteSpace(access[15]) ? null : double.Parse(access[15], CultureInfo.InvariantCulture);

            return weight;
        }
    }
}
