using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Parcer.Model;
using System;
using System.Collections.Generic;
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

            return null;
        }
    }
}
