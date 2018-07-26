﻿using Newtonsoft.Json;

namespace Infocode.Nancy.Metadata.OpenApi.DemoApplication.net45.Model
{
    public class NestedRequestModel
    {
        [JsonProperty("primitiveProperty")]
        public string PrimitiveProperty { get; set; }

        [JsonProperty("simpleModel")]
        public SimpleRequestModel SimpleModel { get; set; }
    }
}
