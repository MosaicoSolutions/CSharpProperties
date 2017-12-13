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

        public Task SaveAsync(string path)
            => SaveAsync(new StreamWriter(path));

        public Task SaveAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () => 
                {
                    using(writer)
                    {
                        foreach (var item in this)
                            await writer.WriteLineAsync($"{item.Key}={item.Value}");

                        await writer.FlushAsync();
                    }
                });

        public Task SaveAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : Task.Run(async () => 
                    {
                        using(var streamWriter = new StreamWriter(stream))
                        {
                            foreach (var item in this)
                                await streamWriter.WriteLineAsync($"{item.Key}={item.Value}");
                            
                            await streamWriter.FlushAsync();
                        }
                    });
    }
}