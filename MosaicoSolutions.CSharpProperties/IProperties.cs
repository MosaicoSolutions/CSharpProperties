using System.Collections;
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
    }
}