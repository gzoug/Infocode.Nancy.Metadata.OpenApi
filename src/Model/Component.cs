using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        [JsonProperty("schemas")]
        public IDictionary<string, JObject> ModelDefinitions { get; set; }
    }
}
