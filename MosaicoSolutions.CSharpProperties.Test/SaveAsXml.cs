using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class SaveAsXml
    {
        [Fact]
        public void SavePropertiesAsXml()
        {
            var content = 
@"name=Saito
role=Turist";

            var properties = Properties.Load(new StringReader(content));

            using(var stringWriter = new StringWriter())
            {
                properties.SaveAsXml(stringWriter);

                var properties2 = Properties.LoadFromXml(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2["name"], properties["name"]);
                Assert.Equal(properties2["role"], properties["role"]);
                Assert.Equal(properties2.Count(), properties.Count());
            }
        }

        [Fact]
        public void SavePropertiesAsXmlInFile()
        {
            var content = 
@"name=Saito
role=Turist";

            var properties = Properties.Load(new StringReader(content));

            var file = $"{TestDirectory.PropertiesDirectoryPath}/properties.xml";

            properties.SaveAsXml(file);

            var properties2 = Properties.LoadFromXml(file);

            Assert.Equal(properties2["name"], properties["name"]);
            Assert.Equal(properties2["role"], properties["role"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }

        [Fact]
        public void PropertiesMustBeEmpty()
        {
            var content = @"";

            var properties = Properties.Load(new StringReader(content));

            using (var stringWriter = new StringWriter())
            {
                properties.SaveAsXml(stringWriter);
                
                var properties2 = Properties.LoadFromXml(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2.Count(), 0);
                Assert.Empty(properties2);
            }
        }

        public void MustSaveAndNotDisposeStream()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = Properties.Load($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

                properties.SaveAsXml(writer);
                
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
                var properties = await Properties.LoadAsync($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

                await properties.SaveAsXmlAsync(writer);
                
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