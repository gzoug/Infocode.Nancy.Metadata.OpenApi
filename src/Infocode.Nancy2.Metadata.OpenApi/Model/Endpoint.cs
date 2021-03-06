﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infocode.Nancy.Metadata.OpenApi.Model
{
    public class Endpoint
    {
        public Endpoint(string name)
        {
            OperationId = name;
        }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("responses")]
        public IDictionary<string, Response> ResponseInfos { get; set; }

        [JsonProperty("parameters")]
        public IList<RequestParameter> RequestParameters { get; set; }

        [JsonProperty("requestBody")]
        public RequestBody RequestBody { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("deprecated")]
        public bool? IsDeprecated { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }
    }
}
