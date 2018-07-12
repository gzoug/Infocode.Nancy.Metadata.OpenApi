using System.Collections.Generic;

namespace Nancy.Metadata.OpenApi.Tests.Fakes
{
    public class FakeRequestComplexModel
    {
        public string Name => "Name";
        public string Description => "Request description";
        public string Loc => "body";
        public bool Required => false;
        public string contentType => "application/json";

        public List<ItemRecord> Records => new List<ItemRecord> { new ItemRecord { } };
    }


    public class ItemRecord
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Item2Record[] List { get; set; }
    }

    // [JsonArray(ItemIsReference = true, ItemTypeNameHandling = TypeNameHandling.All)]
    public class Item2Record
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
