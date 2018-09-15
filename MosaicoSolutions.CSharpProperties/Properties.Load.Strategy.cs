using System;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, string path)
        {
            using (var stream  = new StreamReader(path))
                return LoadFromStrategy(strategy, stream);
        }
        
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, Stream stream)
            => LoadFromStrategy(strategy, new StreamReader(stream));
        
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, TextReader reader)
        {
            if(strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            if(reader == null)
                throw new ArgumentNullException(nameof(reader));
            
            return strategy(reader.ReadToEnd());
        }

        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, string path)
        {
            var stream = new StreamReader(path);

            return Task.Run(async () => 
            {
                using (stream)
                    return await LoadFromStrategyAsync(strategy, stream);
            });
        }
        
        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, Stream stream)
            => LoadFromStrategyAsync(strategy, new StreamReader(stream));

        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, TextReader reader)
        {
            if(strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            if(reader == null)
                throw new ArgumentNullException(nameof(reader));

            return Task.Run(async () =>
            {
                return strategy(await reader.ReadToEndAsync());
            });
        }
    }
}