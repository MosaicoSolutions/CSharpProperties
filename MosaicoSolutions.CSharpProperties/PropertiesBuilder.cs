using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed class PropertiesBuilder
    {
        private Func<string, bool> _validLineHandle;

        private Func<string, KeyValuePair<string, string>> _extractPropertyHandle;

        public PropertiesBuilder WithValidLineHandle(Func<string, bool> validLineHandle)
        {
            _validLineHandle = validLineHandle ?? throw new ArgumentNullException(nameof(validLineHandle));
            return this;
        }

        public PropertiesBuilder WithExtractPropertyHandle(Func<string, KeyValuePair<string, string>> extractPropertyHandle)
        {
            _extractPropertyHandle = extractPropertyHandle ?? throw new ArgumentNullException(nameof(extractPropertyHandle));
            return this;
        }

        public IProperties BuildWithFilePath(string path) 
            => Properties.Load(path, _validLineHandle, _extractPropertyHandle);

        public IProperties BuildWithString(string content)
            => Properties.LoadFromString(content, _validLineHandle, _extractPropertyHandle);

        public IProperties BuildWithStream(Stream stream)
            => Properties.Load(stream, _validLineHandle, _extractPropertyHandle);

        public IProperties BuildWithReader(TextReader reader)
            => Properties.Load(reader, _validLineHandle, _extractPropertyHandle);

        public Task<IProperties> BuildWithFilePathAsync(string path) 
            => Properties.LoadAsync(path, _validLineHandle, _extractPropertyHandle);

        public Task<IProperties> BuildWithStringAsync(string content)
            => Properties.LoadFromStringAsync(content, _validLineHandle, _extractPropertyHandle);

        public Task<IProperties> BuildWithStreamAsync(Stream stream)
            => Properties.LoadAsync(stream, _validLineHandle, _extractPropertyHandle);

        public Task<IProperties> BuildWithReaderAsync(TextReader reader)
            => Properties.LoadAsync(reader, _validLineHandle, _extractPropertyHandle);
    }
}