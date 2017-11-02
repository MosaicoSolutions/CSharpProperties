using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties : IProperties
    {
        public void Save(string path)
            => Save(new StreamWriter(path));

        public void Save(TextWriter writer)
        {
            if(writer == null)
                throw new ArgumentNullException(nameof(writer));
            
            using(writer)
            {
                foreach (var item in this)
                    writer.WriteLine($"{item.Key}={item.Value}");

                writer.Flush();
            }
        }

        public void Save(Stream stream)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));
            
            using(var streamWriter = new StreamWriter(stream))
            {
                foreach (var item in this)
                    streamWriter.WriteLine($"{item.Key}={item.Value}");
                
                streamWriter.Flush();
            }
        }

        public async Task SaveAsync(string path)
            => await SaveAsync(new StreamWriter(path));

        public async Task SaveAsync(TextWriter writer)
        {
            if(writer == null)
                throw new ArgumentNullException(nameof(writer));

           using(writer)
            {
                foreach (var item in this)
                    await writer.WriteLineAsync($"{item.Key}={item.Value}");

                await writer.FlushAsync();
            }
        }

        public async Task SaveAsync(Stream stream)
        {
            if(stream == null)
                throw new ArgumentNullException(nameof(stream));

            using(var streamWriter = new StreamWriter(stream))
            {
                foreach (var item in this)
                    await streamWriter.WriteLineAsync($"{item.Key}={item.Value}");
                
                await streamWriter.FlushAsync();
            }
        }
    }
}