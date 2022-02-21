using Newtonsoft.Json;
using Parcer.Model;
using Parser.Serviсes.Models;
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
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(bodyOfPage);

            var certificate = GetSertificate(myDeserializedClass);

            return certificate;
        }

        private static Certificate GetSertificate(Root root)
        {
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
            var size = GetSize(root, 0);
            var weight = GetWeight(root, 0);
            return certificate;
        }

        private static Product GetProduct(Root root)
        {
            var product = new Product();

            product.Name = root.Elements[1].Elements[1].Value.ToString();

            return product;
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



            return chemical;
        }
    }
}
