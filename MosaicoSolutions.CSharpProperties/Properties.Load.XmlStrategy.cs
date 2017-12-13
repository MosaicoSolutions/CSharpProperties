using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        private const string PropertiesXsd = 
        @"";

        public static PropertiesStrategy XmlStrategy()
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

                return Properties.Of(query);
            };

        public static IProperties LoadFromXml(string path)
            => IsXmlFile(path)
                ? LoadFromXml(new StreamReader(path))
                : throw new IOException("The file must have the extension '.xml'.");

        private static bool IsXmlFile(string path)
            => path.EndsWith(".xml");

        public static IProperties LoadFromXml(TextReader reader)
        {
            if(reader == null)
                throw new ArgumentNullException(nameof(reader));

            using(reader)
                return LoadFromStrategy(XmlStrategy(), reader.ReadToEnd());
        }

        public static IProperties LoadFromXml(Stream stream)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            using(var streamReader = new StreamReader(stream))
                return LoadFromStrategy(XmlStrategy(), streamReader.ReadToEnd());
        }

        public static Task<IProperties> LoadFromXmlAsync(string path)
            => IsXmlFile(path)
                ? LoadFromXmlAsync(new StreamReader(path))
                : throw new IOException("The file must have the extension '.xml'.");

        public static Task<IProperties> LoadFromXmlAsync(TextReader reader)
            => reader == null
                ? throw new ArgumentNullException(nameof(reader))
                : Task.Run(async () => 
                {
                    using(reader)
                        return await LoadFromStrategyAsync(XmlStrategy(), await reader.ReadToEndAsync());
                });

        public static Task<IProperties> LoadFromXmlAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : Task.Run(async () => 
                {
                    using(var streamReader = new StreamReader(stream))
                        return await LoadFromStrategyAsync(XmlStrategy(), await streamReader.ReadToEndAsync());
                });
    }
}