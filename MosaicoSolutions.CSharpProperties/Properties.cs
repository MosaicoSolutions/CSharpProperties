using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        private readonly Dictionary<string, string> _properties;

        public static IProperties Of(IEnumerable<KeyValuePair<string, string>> properties)
            => new Properties(properties.ToDictionary(property => property.Key, property => property.Value));

        public static IProperties Empty()
            => Of(Enumerable.Empty<KeyValuePair<string, string>>());

        private Properties(IDictionary<string, string> properties) 
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