using Newtonsoft.Json;

namespace Parser.Serviсes.Models.NlmkJson
{
    public class Root
    {
        [JsonProperty("elements")]
        public List<Element> Elements { get; set; }
    }
}
