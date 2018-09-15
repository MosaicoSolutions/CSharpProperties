using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        private static readonly string[] IgnoredCharacters = { "'\''", ";", "#" };

        public static IProperties Load(string path)
            => Load(path, IsValidLine, ExtractPropertyFromLine);
        
        public static IProperties LoadFromString(string content)
            => LoadFromString(content, IsValidLine, ExtractPropertyFromLine);

        public static IProperties Load(TextReader reader)
            => Load(reader, IsValidLine, ExtractPropertyFromLine);

        public static IProperties Load(Stream stream)
            => Load(stream, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(string path)
            => LoadAsync(path, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadFromStringAsync(string content)
            => LoadFromStringAsync(content, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(TextReader reader)
            => LoadAsync(reader, IsValidLine, ExtractPropertyFromLine);

        public static Task<IProperties> LoadAsync(Stream stream)
            => LoadAsync(stream, IsValidLine, ExtractPropertyFromLine);

        public static IProperties Load(string path,
                                       Func<string, bool> isValidLine,
                                       Func<string, KeyValuePair<string, string>> extractProperty)
        {
            using (var stream = new StreamReader(path))
                return Load(stream, isValidLine, extractProperty);
        }
        
        public static IProperties LoadFromString(string content,
                                                 Func<string, bool> isValidLine,
                                                 Func<string, KeyValuePair<string, string>> extractProperty)
        {
            using (var reader = new StringReader(content))
                return Load(reader, isValidLine, extractProperty);
        }

        public static IProperties Load(Stream stream,
                                       Func<string, bool> isValidLine,
                                       Func<string, KeyValuePair<string, string>> extractProperty)
            => Load(new StreamReader(stream), isValidLine, extractProperty);
        
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
            string line;

            while((line = reader.ReadLine()) != null)
                if(isValidLine(line))
                {
                    var keyValuePair = extractProperty(line);

                    if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                }

            return Of(dictionary);
        }

        public static Task<IProperties> LoadAsync(string path, 
                                                  Func<string, bool> isValidLine, 
                                                  Func<string, KeyValuePair<string, string>> extractProperty)
        {
            var stream = new StreamReader(path);

            return Task.Run(async () => 
            {
                using (stream)
                    return await LoadAsync(stream, isValidLine, extractProperty);
            });
        }

        public static Task<IProperties> LoadFromStringAsync(string content, 
                                                            Func<string, bool> isValidLine, 
                                                            Func<string, KeyValuePair<string, string>> extractProperty)
        {
            var reader = new StringReader(content);

            return Task.Run(async () => 
            {
                using (reader)
                    return await LoadAsync(reader, isValidLine, extractProperty);
            });
        }

        public static Task<IProperties> LoadAsync(Stream stream,
                                                  Func<string, bool> isValidLine,
                                                  Func<string, KeyValuePair<string, string>> extractProperty)
            => LoadAsync(new StreamReader(stream), isValidLine, extractProperty);

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
                string line;

                while((line = await reader.ReadLineAsync()) != null)
                    if(isValidLine(line))
                    {
                        var keyValuePair = extractProperty(line);

                        if (!string.IsNullOrEmpty(keyValuePair.Key) && !dictionary.ContainsKey(keyValuePair.Key))
                            dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }

                return Of(dictionary);
            });
        }

        private static bool IsValidLine(string line)
            => !string.IsNullOrEmpty(line) && !StartsWithIgnoredCharacters(line);

        private static bool StartsWithIgnoredCharacters(string line)
            => IgnoredCharacters.Any(line.StartsWith);

        private static KeyValuePair<string, string> ExtractPropertyFromLine(string line)
        {
            var tokens = line.Split('=');
            return tokens.Length == 2
                    ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
                    : new KeyValuePair<string, string>();
        }
    }
}