using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infocode.Nancy.Metadata.OpenApi.Model
{
    public class Component
    {
        [JsonProperty("schemas")]
        public IDictionary<string, JObject> ModelDefinitions { get; set; }
    }
}
