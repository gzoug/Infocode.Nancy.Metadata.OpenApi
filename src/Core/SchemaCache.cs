using Newtonsoft.Json.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nancy.Metadata.OpenApi.Core
{
    public static class SchemaCache
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

        private static JsonSchemaGenerator GetSchemaGenerator()
        {
            return new JsonSchemaGenerator(_settings);
        }

        private static void AddToCache(string key, JObject value)
        {
            if (!Cache.ContainsKey(key))
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

            jo.Remove("definitions");
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
                var generator = GetSchemaGenerator();
                var schema = generator.GenerateAsync(type).Result;
                AddToCache(typeName, JObject.Parse(schema.ToJson()));
                CacheSchemaDefinitions(schema);
            }
            return typeName;
        }

        private static void CacheSchemaDefinitions(JsonSchema4 schema)
        {
            foreach (var d in schema.Definitions)
            {
                CacheSchemaDefinitions(d.Value); 
                if (!Cache.ContainsKey(d.Key))
                {
                    try
                    {                        
                        AddToCache(d.Key, JObject.Parse(d.Value.ToJson()));
                    }
                    catch (Exception exc)
                    {
                        // skip model if could not convert to jobject
                        AddToCache(d.Key, JObject.Parse("{}")); // JObject.Parse("{\"exception\":\"" + exc.ToString() + Environment.NewLine + exc.StackTrace + "\"}"));
                    }
                }
            }
        }
    }
}
