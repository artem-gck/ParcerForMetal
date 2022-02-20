using Newtonsoft.Json;
using Parcer.Model;
using Parser.Serviсes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Services.Logic.ChainOfHosts
{
    public class NlmkHandler : Handler
    {
        private const string Nlmk = "nlmk";

        public override async Task<Certificate> HandleRequestAsync(Uri link)
        {
            if (link.PathAndQuery.Contains(Nlmk))
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

            var a = await _httpClient.GetAsync($"{identyOfCertificate[2..]}?lang=ru");
            var b = await a.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(b);

            return null;
        }
    }
}
