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
        private static JsonSchemaResolver _resolver;
        private static JsonSchema4 _schema;
        private static JsonSchemaGenerator _generator;

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
            if (_schema == null)
            {
                _schema = new JsonSchema4();
            }
            if (_resolver == null)
            {
                _resolver = new JsonSchemaResolver(_schema, _settings);
            }
            if (_generator == null)
            {
                _generator = new JsonSchemaGenerator(_settings);
            }
            return _generator;
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
                var firstRun = (_schema == null);
                var generator = GetSchemaGenerator();

                if (firstRun)
                {
                    generator.GenerateAsync(type, null, _schema, _resolver).Wait(); // generator.GenerateAsync(type).Result;
                }
                else
                {
                    var x = generator.GenerateAsync(type, _resolver).Result; // generator.GenerateAsync(type).Result;
                }

                var typeSchema = _schema;
                if (typeSchema != null)
                {
                    AddToCache(typeName, JObject.Parse(typeSchema.ToJson()));
                }

                _schema = null;
                _generator = null;
                _resolver = null;
            }
            return typeName;
        }
    }
}
