using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infocode.Nancy.Metadata.OpenApi.Model
{
    public class Response
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("headers")]
        public IDictionary<string, TypeDefinition> Headers { get; set; }

        [JsonProperty("schema")]
        public SchemaRef Schema { get; set; }
    }
}
