using System;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        public static IProperties LoadFromStrategy(PropertiesStrategy strategy, string content)
        {
            if(strategy == null)
                throw new ArgumentException(nameof(strategy));

            if(content == null)
                throw new ArgumentException(nameof(content));
            
            return strategy(content);
        }

        public static Task<IProperties> LoadFromStrategyAsync(PropertiesStrategy strategy, string content)
        {
            if(strategy == null)
                throw new ArgumentException(nameof(strategy));

            if(content == null)
                throw new ArgumentException(nameof(content));

            return Task.Run(() => strategy(content));
        }
    }
}