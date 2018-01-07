using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public interface IProperties : IEnumerable<KeyValuePair<string, string>>
    {
        string this[string key] { get; set; }

        bool ContainsKey(string key);

        void Add(string key, string value);

        void Add(KeyValuePair<string, string> valuePair);

        IDictionary<string, string> ToDictionary();

        IEnumerable<string> Keys { get; }

        IEnumerable<string> Values { get; }

        string Get(string key);

        void Save(string path);

        void Save(TextWriter writer);

        void Save(Stream stream);

        Task SaveAsync(string path);

        Task SaveAsync(TextWriter writer);

        Task SaveAsync(Stream stream);

        void SaveAsXml(string path);

        void SaveAsXml(TextWriter writer);

        void SaveAsXml(Stream stream);
        
        Task SaveAsXmlAsync(string path);

        Task SaveAsXmlAsync(TextWriter writer);

        Task SaveAsXmlAsync(Stream stream);

        void SaveAsJson(string path);

        void SaveAsJson(TextWriter writer);

        void SaveAsJson(Stream stream);
        
        Task SaveAsJsonAsync(string path);

        Task SaveAsJsonAsync(TextWriter writer);

        Task SaveAsJsonAsync(Stream stream);

        void SaveAsCsv(string path);

        void SaveAsCsv(TextWriter writer);

        void SaveAsCsv(Stream stream);
        
        Task SaveAsCsvAsync(string path);

        Task SaveAsCsvAsync(TextWriter writer);

        Task SaveAsCsvAsync(Stream stream);
    }
}