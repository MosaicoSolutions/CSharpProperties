using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        private const string PropertiesJsonSchema = 
@"{
    'type': 'array',
    'items': {
        'type': 'object',
        'properties': {
            'key': {
                'type': 'string',
                'required': true
            },
            'value': {
                'type': 'string',
                'required': true
            }
        }
    }
}";

        public static PropertiesStrategy JsonStrategy
            => content =>
            {
                var schema = JSchema.Parse(PropertiesJsonSchema);
                var properties = JArray.Parse(content);

                if (!properties.IsValid(schema, out IList<string> messages))
                    throw new JsonSchemaValidationException(messages);
                
                var query = from property in properties.AsJEnumerable()
                    let key = (string) property["key"]
                    let value = (string) property["value"]
                    select new KeyValuePair<string, string>(key, value);

                return Of(query);
            };

        public static IProperties LoadFromJson(string path)
            => IsJsonFile(path)
                ? LoadFromStrategy(JsonStrategy, path)
                : throw new IOException("The file must have the extension '.json'.");

        private static bool IsJsonFile(string path)
            => path.EndsWith(".json");

        public static IProperties LoadFromJson(TextReader reader)
            => LoadFromStrategy(JsonStrategy, reader);

        public static IProperties LoadFromJson(Stream stream)
            => LoadFromStrategy(JsonStrategy, stream);

        public static Task<IProperties> LoadFromJsonAsync(string path)
            => IsJsonFile(path)
                ? LoadFromStrategyAsync(JsonStrategy, path)
                : throw new IOException("The file must have the extension '.json'.");

        public static Task<IProperties> LoadFromJsonAsync(TextReader reader)
            => LoadFromStrategyAsync(JsonStrategy, reader);

        public static Task<IProperties> LoadFromJsonAsync(Stream stream)
            => LoadFromStrategyAsync(JsonStrategy, stream);
    }
}