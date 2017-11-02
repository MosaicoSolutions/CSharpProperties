using System.Collections;
using System.Collections.Generic;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        private readonly Dictionary<string, string> _properties;

        private Properties(Dictionary<string, string> properties) 
            => _properties = new Dictionary<string, string>(properties);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            => _properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();

        public string this[string key]
        {
            get => _properties[key];
            set => _properties[key] = value;
        }

        public bool ContainsKey(string key) 
            => _properties.ContainsKey(key);

        public void Add(string key, string value)
            => _properties.Add(key, value);

        public void Add(KeyValuePair<string, string> valuePair)
            => Add(valuePair.Key, valuePair.Value);

        public IDictionary<string, string> ToDictionary()
            => new Dictionary<string, string>(_properties);

        public IEnumerable<string> Keys => new Dictionary<string, string>.KeyCollection(_properties);

        public IEnumerable<string> Values => new Dictionary<string, string>.ValueCollection(_properties);

        public string Get(string key) => this[key];
    }
}