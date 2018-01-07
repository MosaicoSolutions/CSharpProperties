using System.IO;
using System.Linq;
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

            var file = @"C:\temp\properties.xml";

            properties.SaveAsXml(file);

            var properties2 = Properties.LoadFromXml(file);

            Assert.Equal(properties2["name"], properties["name"]);
            Assert.Equal(properties2["role"], properties["role"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }
    }
}