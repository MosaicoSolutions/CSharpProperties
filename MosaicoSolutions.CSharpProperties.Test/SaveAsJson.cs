using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MosaicoSolutions.CSharpProperties.Test.IO;
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

            var file = $"{TestDirectory.PropertiesDirectoryPath}/properties2.json";
            properties.SaveAsJson(file);

            var properties2 = Properties.LoadFromJson(file);
            
            Assert.Equal(properties2["host"], properties["host"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }

        [Fact]
        public void PropertiesMustBeEmpty()
        {
            var content = @"";

            var properties = Properties.Load(new StringReader(content));

            using (var stringWriter = new StringWriter())
            {
                properties.SaveAsJson(stringWriter);

                var properties2 = Properties.LoadFromJson(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2.Count(), 0);
                Assert.Empty(properties2);
            }
        }

        [Fact]
        public void MustSaveAndNotDisposeStream()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = Properties.Load($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

                properties.SaveAsJson(writer);
                
                writer.WriteLine("prop2=value2");
                writer.Flush();

                Assert.Contains("prop2=value2", writer.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        [Fact]
        public async Task MustSaveAndNotDisposeStreamAsync()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = await Properties.LoadAsync($"{TestDirectory.PropertiesDirectoryPath}/userProperties.properties");

                await properties.SaveAsJsonAsync(writer);
                
                await writer.WriteLineAsync("prop2=value2");
                await writer.FlushAsync();

                Assert.Contains("prop2=value2", writer.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}