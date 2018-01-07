using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        public static PropertiesStrategy XmlStrategy
            => content => 
            {
                var schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(PropertiesXsd)));

                var document = XDocument.Parse(content);
                document.Validate(schemas, (_, e) => throw new XmlSchemaValidationException(e.Message));

                var query = from property in document.Root.Elements()
                            let key = (string) property.Attribute("key")
                            let value = (string) property.Attribute("value")
                            select new KeyValuePair<string, string>(key, value);

                return Of(query);
            };

        public static IProperties LoadFromXml(string path)
            => IsXmlFile(path)
                ? LoadFromXml(new StreamReader(path))
                : throw new IOException("The file must have the extension '.xml'.");

        private static bool IsXmlFile(string path)
            => path.EndsWith(".xml");

        public static IProperties LoadFromXml(TextReader reader)
            => LoadFromStrategy(XmlStrategy, reader);

        public static IProperties LoadFromXml(Stream stream)
            => LoadFromStrategy(XmlStrategy, stream);

        public static Task<IProperties> LoadFromXmlAsync(string path)
            => IsXmlFile(path)
                ? LoadFromXmlAsync(new StreamReader(path))
                : throw new IOException("The file must have the extension '.xml'.");

        public static Task<IProperties> LoadFromXmlAsync(TextReader reader)
            => LoadFromStrategyAsync(XmlStrategy, reader);

        public static Task<IProperties> LoadFromXmlAsync(Stream stream)
            => LoadFromStrategyAsync(XmlStrategy, stream);
        
        private const string PropertiesXsd = 
        @"<?xml version='1.0' encoding='UTF-8'?>
<xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'
	attributeFormDefault='unqualified' elementFormDefault='qualified'>

	<xs:element name='properties' type='propertiesType' />

	<xs:complexType name='propertiesType'>
		<xs:sequence>
			<xs:element type='propertyType' name='property' maxOccurs='unbounded' minOccurs='0' />
		</xs:sequence>
	</xs:complexType>

	<xs:complexType name='propertyType'>
		<xs:simpleContent>
			<xs:extension base='xs:string'>
				<xs:attribute type='keyType' name='key' use='required' />
				<xs:attribute type='valueType' name='value' use='required' />
			</xs:extension>
		</xs:simpleContent>
	</xs:complexType>

	<xs:simpleType name='keyType'>
		<xs:restriction base='xs:string'>
			<xs:minLength value='1' />
		</xs:restriction>
	</xs:simpleType>
	
	<xs:simpleType name='valueType'>
		<xs:restriction base='xs:string'>
			<xs:minLength value='0' />
		</xs:restriction>
	</xs:simpleType>
	
</xs:schema>";

    }
}