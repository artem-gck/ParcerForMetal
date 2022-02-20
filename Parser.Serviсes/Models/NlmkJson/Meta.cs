using Newtonsoft.Json;

namespace Parser.Serviсes.Models.NlmkJson
{
    public class Meta
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("cert_pos")]
        public string CertPos { get; set; }
    }
}
