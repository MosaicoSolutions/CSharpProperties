using System.IO;
using System.Xml.Schema;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class LoadPropertiesFromXml
    {
        [Fact]
        public void LoadFromXml()
        {
            var xml = 
            @"<?xml version='1.0' encoding='UTF-8'?>
            <properties>
                <property key='email' value='ariadne@gmail.com'/>
            </properties>";

            var properties = Properties.LoadFromXml(new StringReader(xml));

            Assert.Equal(properties["email"], "ariadne@gmail.com");
        }

        [Fact]
        public void LoadFromXmlMustFailValueNotSupplied()
            => Assert.Throws<XmlSchemaValidationException>(() => 
            {
                var xml = 
                @"<?xml version='1.0' encoding='UTF-8'?>
                <properties>
                    <property key='name' value='Eames' />
                    <property key='role' value='forger' />
                    <property key='age' />
                </properties>";

                var properties = Properties.LoadFromXml(new StringReader(xml));
            });

        [Fact]
        public void LoadFromXmlAndDoNotDisposeStream()
        {
            var sourceFile = $"{TestDirectory.PropertiesDirectoryPath}/properties.xml";
            var targetFile = $"{TestDirectory.PropertiesDirectoryPath}/properties_copy.xml";

            File.Copy(sourceFileName: sourceFile, destFileName: targetFile, overwrite: true);

            using (var stream = File.Open(targetFile, FileMode.Open))
            {
                var properties = Properties.LoadFromXml(stream);

                stream.WriteByte(byte.MinValue);
                stream.WriteByte(byte.MaxValue);
            }
        }
    }
}