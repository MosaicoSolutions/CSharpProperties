using System;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, string path)
            => LoadFromStrategy(strategy, new StreamReader(path));
        
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, Stream stream)
            => LoadFromStrategy(strategy, new StreamReader(stream));
        
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, TextReader reader)
        {
            if(strategy == null)
                throw new ArgumentException(nameof(strategy));

            if(reader == null)
                throw new ArgumentException(nameof(reader));

            using(reader)
                return strategy(reader.ReadToEnd());
        }

        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, string path)
            => LoadFromStrategyAsync(strategy, new StreamReader(path));
        
        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, Stream stream)
            => LoadFromStrategyAsync(strategy, new StreamReader(stream));

        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, TextReader reader)
        {
            if(strategy == null)
                throw new ArgumentException(nameof(strategy));

            if(reader == null)
                throw new ArgumentException(nameof(reader));

            return Task.Run(async () =>
            {
                using(reader)
                    return strategy(await reader.ReadToEndAsync());
            });
        }
    }
}