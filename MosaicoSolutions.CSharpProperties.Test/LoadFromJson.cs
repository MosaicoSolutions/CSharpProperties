using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class LoadPropertiesFromJson
    {
        [Fact]
        public void EmptyProperties()
        {
            const string json = @"[]";

            var properties = Properties.LoadFromJson(new StringReader(json));
            Assert.Empty(properties);
            Assert.Equal(properties.Count(), 0); 
        }

        [Fact]
        public void InvalidJson()
            => Assert.Throws<JsonReaderException>(() =>
            {
                const string json = @"{'email' = 'mail@mail.com'}";

                var properties = Properties.LoadFromJson(new StringReader(json));
            });

        [Fact]
        public void InvalidFile()
            => Assert.Throws<IOException>(() =>
            {
                var properties = Properties.LoadFromJson(@"C:\temp\properties.txt");
            });

        [Fact]
        public void InvalidJsonSchema()
            => Assert.Throws<JsonSchemaValidationException>(() =>
            {
                var properties = Properties.LoadFromJson(File.Open(@"C:\temp\propertiesInvalidSchema.json", FileMode.Open));
            });

        [Fact]
        public void JsonValid()
        {
            const string json = @"[
                {
                    'key': 'host', 'value':'localhost'
                },
                {
                    'key': 'database', 'value':'inception'
                }
            ]";

            var properties = Properties.LoadFromJson(new StringReader(json));

            Assert.Equal(properties.Count(), 2);
            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["database"], "inception");
        }
    }
}