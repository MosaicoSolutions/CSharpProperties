using System;
using System.Collections.Generic;
using System.Linq;

namespace MosaicoSolutions.CSharpProperties.Extensions
{
    public static class PropertiesExtensions
    {
        public static string GetOrEmpty(this IProperties properties, string key)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            return properties.ContainsKey(key)
                    ? properties[key]
                    : string.Empty;
        }

        public static string GetOrElse(this IProperties properties, string key, string orElse)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            return properties.ContainsKey(key)
                    ? properties[key]
                    : orElse;
        }
    }
}