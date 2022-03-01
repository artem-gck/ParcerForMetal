using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Parcer.Model;
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

            var number = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("Сертификат №:")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            var date = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("Сертификат №:")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[3].Trim();
            var recipient = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("ГРУЗОПОЛУЧАТЕЛЬ, АДРЕС")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            var specificationNumber = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("СПЕЦИФИКАЦИЯ №")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            var recipientCountry = document.QuerySelectorAll("tr").Where(element => element.Text().Contains("СТРАНА НАЗНАЧЕНИЯ")).Last().GetElementsByTagName("td").Select(element => element.Text()).ToList()[1].Trim();
            var gosts = document.QuerySelectorAll("td").Where(element => element.Text().Contains("ГОСТ")).Select(element => element.Text()).Last().Trim();
            var typeOfPackaging = document.QuerySelectorAll("tbody").Where(element => element.Text().Contains("НАИМЕНОВАНИЕ И КОД ТОВАРА")).Last().Children.Last().Children.First().Children.First().Children.First().Children.First().Text().Trim();

            var c = GetChemicalComposition(document);
            var s = GetSize(document, 0);
            var w = GetWeight(document, 0);

            return null;
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

            size.Width = string.IsNullOrWhiteSpace(access[11]) ? null : double.Parse(access[11], CultureInfo.InvariantCulture);
            size.Thickness = string.IsNullOrWhiteSpace(access[9]) ? null : double.Parse(access[23], CultureInfo.InvariantCulture);
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
