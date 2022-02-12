// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class Meta
{
    [JsonProperty("product_code")]
    public string ProductCode { get; set; }

    [JsonProperty("cert_pos")]
    public string CertPos { get; set; }
}

public class Body
{
    [JsonProperty("@meta")]
    public Meta Meta { get; set; }

    [JsonProperty("tr")]
    public List<string> Tr { get; set; }
}

public class Element
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("sortField")]
    public string SortField { get; set; }

    [JsonProperty("head")]
    public List<string> Head { get; set; }

    [JsonProperty("body")]
    public List<Body> Body { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("value")]
    public object Value { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("elements")]
    public List<Element> Elements { get; set; }
}

public class Root
{
    [JsonProperty("elements")]
    public List<Element> Elements { get; set; }
}

