using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;

namespace Infocode.Nancy.Metadata.OpenApi.Core
{
    public class SchemaCache
    {
        public static IDictionary<string, JObject> Cache = new Dictionary<string, JObject>();

        private static IList<Type> cachedTypeNames = new List<Type>();

        private static JsonSchemaGeneratorSettings _settings;

        static SchemaCache()
        {
            _settings = new JsonSchemaGeneratorSettings()
            {
                SchemaType = SchemaType.OpenApi3,
                FlattenInheritanceHierarchy = true,
                // DefaultEnumHandling = EnumHandling.String,
                TypeNameGenerator = new TypeNameGenerator(),
                SchemaNameGenerator = new TypeNameGenerator(),
                AllowReferencesWithProperties = true,
                IgnoreObsoleteProperties = false,

            };
        }

        private static void AddToCache(string key, JObject value)
        {
            if (key != null && !Cache.ContainsKey(key))
            {
                ClearSchemaNodes(value);
                Cache.Add(key, value);
            }
        }

        private static void ClearSchemaNodes(JObject jo)
        {
            if (jo == null)
            {
                return;
            }
            jo.Remove("$schema");
            foreach (var o in jo.Properties().Where(x => x.Type == JTokenType.Object))
            {
                ClearSchemaNodes((JObject)(o.Value));
            }
        }

        public static string AddSchema(Type type)
        {
            string typeName = type.FullName;

            if (!Cache.ContainsKey(typeName))
            {
                var generator = new JsonSchemaGenerator(_settings);
                var schema = generator.GenerateAsync(type).Result;
                if (schema != null)
                {
                    AddToCache(typeName, JObject.Parse(schema.ToJson()));
                }
            }
            return typeName;
        }
    }
}
