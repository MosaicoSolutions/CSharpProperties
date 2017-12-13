using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        private static string[] IgnoredCharacters = { "'\''", ";", "#" };

        public static IProperties Of(IEnumerable<KeyValuePair<string, string>> properties)
            => new Properties(properties.ToDictionary(property => property.Key, property => property.Value));

        public static IProperties Load(string path)
            => Load(path, IsValidLine, ExtractPropertyFromLine);
        
        public static IProperties Load(TextReader reader)
            => Load(reader, IsValidLine, ExtractPropertyFromLine);

        public static IProperties Load(Stream stream)
            => Load(stream, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(string path)
            => LoadAsync(path, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(TextReader reader)
            => LoadAsync(reader, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(Stream stream)
            => LoadAsync(stream, IsValidLine, ExtractPropertyFromLine);

        public static IProperties Load(string path,
                                       Func<string, bool> isValidLine,
                                       Func<string, KeyValuePair<string, string>> extractProperty)
        {
            if(path == null)
                throw new ArgumentNullException(nameof(path));

            if(isValidLine == null)
                throw new ArgumentNullException(nameof(isValidLine));

            if(extractProperty == null)
                throw new ArgumentNullException(nameof(extractProperty));

            return Load(new StreamReader(path), isValidLine, extractProperty);
        }

        public static IProperties Load(TextReader reader,
                                       Func<string, bool> isValidLine,
                                       Func<string, KeyValuePair<string, string>> extractProperty)
        {
            if(reader == null)
                throw new ArgumentNullException(nameof(reader));

            if(isValidLine == null)
                throw new ArgumentNullException(nameof(isValidLine));

            if(extractProperty == null)
                throw new ArgumentNullException(nameof(extractProperty));

            var dictionary = new Dictionary<string, string>();
            var line = string.Empty;

            using(reader)
                while((line = reader.ReadLine()) != null)
                {
                    if(isValidLine(line))
                    {
                        var keyValuePair = extractProperty(line);

                        if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }

            return new Properties(dictionary);
        }

        public static IProperties Load(Stream stream,
                                       Func<string, bool> isValidLine,
                                       Func<string, KeyValuePair<string, string>> extractProperty)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            if(isValidLine == null)
                throw new ArgumentNullException(nameof(isValidLine));

            if(extractProperty == null)
                throw new ArgumentNullException(nameof(extractProperty));

            var dictionary = new Dictionary<string, string>();
            var line = string.Empty;
            
            using(var streamReader = new StreamReader(stream))
                while(!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();

                    if(isValidLine(line))
                    {
                        var keyValuePair = extractProperty(line);

                        if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }

            return new Properties(dictionary);
        }

        public static Task<IProperties> LoadAsync(string path, 
                                                  Func<string, bool> isValidLine, 
                                                  Func<string, KeyValuePair<string, string>> extractProperty)
            =>  LoadAsync(new StreamReader(path), isValidLine, extractProperty);

        public static Task<IProperties> LoadAsync(TextReader reader, 
                                                  Func<string, bool> isValidLine, 
                                                  Func<string, KeyValuePair<string, string>> extractProperty)
        {
            if(reader == null)
                throw new ArgumentNullException(nameof(reader));

            if(isValidLine == null)
                throw new ArgumentNullException(nameof(isValidLine));

            if(extractProperty == null)
                throw new ArgumentNullException(nameof(extractProperty));

            return Task.Run<IProperties>(async () =>
            {
                var dictionary = new Dictionary<string, string>();
                var line = string.Empty;

                using(reader)
                    while((line = await reader.ReadLineAsync()) != null)
                    {
                        if(isValidLine(line))
                        {
                            var keyValuePair = extractProperty(line);

                            if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }

                return new Properties(dictionary);
            });
        }

        public static Task<IProperties> LoadAsync(Stream stream,
                                                  Func<string, bool> isValidLine,
                                                  Func<string, KeyValuePair<string, string>> extractProperty)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            if(isValidLine == null)
                throw new ArgumentNullException(nameof(isValidLine));

            if(extractProperty == null)
                throw new ArgumentNullException(nameof(extractProperty));

            return Task.Run<IProperties>(async () => 
            {
                var dictionary = new Dictionary<string, string>();
                var line = string.Empty;
            
                using(var streamReader = new StreamReader(stream))
                    while(!streamReader.EndOfStream)
                    {
                        line = await streamReader.ReadLineAsync();

                        if(isValidLine(line))
                        {
                            var keyValuePair = extractProperty(line);

                            if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }

                return new Properties(dictionary);
            });
        }

        private static bool IsValidLine(string line)
            => !string.IsNullOrEmpty(line) && !ContainsIgnoredCharacters(line);

        private static bool ContainsIgnoredCharacters(string line)
            => IgnoredCharacters.Any(line.Contains);

        private static KeyValuePair<string, string> ExtractPropertyFromLine(string line)
        {
            var tokens = line.Split('=');
            return tokens.Length == 2
                    ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
                    : new KeyValuePair<string, string>();
        }
    }
}