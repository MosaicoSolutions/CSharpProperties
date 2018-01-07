using System.IO;
using System.Linq;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class SaveAsJson
    {
        [Fact]
        public void SavePropertiesAsJson()
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

            using(var stringWriter = new StringWriter())
            {
                properties.SaveAsJson(stringWriter);

                var properties2 = Properties.LoadFromJson(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2["host"], properties["host"]);
                Assert.Equal(properties2["database"], properties["database"]);
                Assert.Equal(properties2.Count(), properties.Count());
            }
        }

        [Fact]
        public void SavePropertiesAsJsonFile()
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

            var file = @"C:\temp\properties.json";
            properties.SaveAsJson(file);

            var properties2 = Properties.LoadFromJson(file);
            
            Assert.Equal(properties2["host"], properties["host"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }
    }
}