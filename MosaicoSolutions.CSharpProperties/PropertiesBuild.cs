using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed class PropertiesBuild
    {
        private Func<string, bool> _validLineHandle;

        private Func<string, KeyValuePair<string, string>> _extractPropertyHandle;

        public static PropertiesBuild NewPropertiesBuild()
            => new PropertiesBuild();

        public PropertiesBuild WithValidLineHandle(Func<string, bool> validLineHandle)
        {
            _validLineHandle = validLineHandle ?? throw new ArgumentNullException(nameof(validLineHandle));
            return this;
        }

        public PropertiesBuild WithExtractPropertyHandle(Func<string, KeyValuePair<string, string>> extractPropertyHandle)
        {
            _extractPropertyHandle = extractPropertyHandle ?? throw new ArgumentNullException(nameof(extractPropertyHandle));
            return this;
        }

        public IProperties BuildWithPathFile(string path) 
            => Properties.Load(path, _validLineHandle, _extractPropertyHandle);

        public IProperties BuildWithStream(Stream stream)
            => Properties.Load(stream, _validLineHandle, _extractPropertyHandle);

        public IProperties BuildWithTextReader(TextReader reader)
            => Properties.Load(reader, _validLineHandle, _extractPropertyHandle);

        public async Task<IProperties> BuildWithPathFileAsync(string path) 
            => await Properties.LoadAsync(path, _validLineHandle, _extractPropertyHandle);

        public async Task<IProperties> BuildWithStreamAsync(Stream stream)
            => await Properties.LoadAsync(stream, _validLineHandle, _extractPropertyHandle);

        public async Task<IProperties> BuildWithTextReaderAsync(TextReader reader)
            => await Properties.LoadAsync(reader, _validLineHandle, _extractPropertyHandle);
    }
}