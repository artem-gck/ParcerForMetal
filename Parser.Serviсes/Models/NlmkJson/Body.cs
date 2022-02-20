using Newtonsoft.Json;

namespace Parser.Serviсes.Models.NlmkJson
{
    public class Body
    {
        [JsonProperty("@meta")]
        public Meta Meta { get; set; }

        [JsonProperty("tr")]
        public List<string> Tr { get; set; }
    }
}
