using Newtonsoft.Json;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Item
    {
        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }
    }
}
