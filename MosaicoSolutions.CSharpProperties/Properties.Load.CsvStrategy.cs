using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        private const char Delimiter = ';';

        public static PropertiesStrategy CsvStrategy
            => content =>
            {
                var lines = new List<string>();

                using(var reader = new StringReader(content))
                {
                    while(reader.Peek() >= 0)
                    {
                        var line = reader.ReadLine();
                        if(string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                            continue;

                        lines.Add(line.Replace(" ", ""));
                    }
                }

                var query = from property in (from property in lines
                                              where !property.StartsWith("#") && property.Contains(Delimiter)
                                              let tokens = property.Split(Delimiter)
                                              select tokens.Length == 2
                                                        ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
                                                        : new KeyValuePair<string, string>())
                            where !string.IsNullOrEmpty(property.Key)
                            select property;

                return Of(query);            
            };
        
        public static IProperties LoadFromCsv(string path)
            => IsCsvFile(path)
                ? LoadFromCsv(new StreamReader(path))
                : throw new IOException("The file must have the extension '.csv'.");

        private static bool IsCsvFile(string path)
            => path.EndsWith(".csv");

        public static IProperties LoadFromCsv(TextReader reader)
            => LoadFromStrategy(CsvStrategy, reader);

        public static IProperties LoadFromCsv(Stream stream)
            => LoadFromStrategy(CsvStrategy, stream);

        public static Task<IProperties> LoadFromCsvAsync(string path)
            => IsCsvFile(path)
                ? LoadFromCsvAsync(new StreamReader(path))
                : throw new IOException("The file must have the extension '.csv'.");

        public static Task<IProperties> LoadFromCsvAsync(TextReader reader)
            => LoadFromStrategyAsync(CsvStrategy, reader);

        public static Task<IProperties> LoadFromCsvAsync(Stream stream)
            => LoadFromStrategyAsync(CsvStrategy, stream);

    }
}