using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using MosaicoSolutions.CSharpProperties.Extensions;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            
            stream.WriteProperties(this);
        }

        public void Save(string path)
        {
            using (var stream = new StreamWriter(path))
                Save(stream);
        }
        
        public void Save(TextWriter writer)
        {
            if(writer == null)
                throw new ArgumentNullException(nameof(writer));
                
            foreach (var item in this)
                writer.WriteLine($"{item.Key}={item.Value}");

            writer.Flush();
        }

        public Task SaveAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : stream.WritePropertiesAsync(this);

        public Task SaveAsync(string path)
            => path == null
                ? throw new ArgumentNullException(nameof(path))
                : Task.Run(async () => 
                {
                    using (var stream = new StreamWriter(path))
                        await SaveAsync(stream);
                });
        
        public Task SaveAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () => 
                {
                    foreach (var item in this)
                        await writer.WriteLineAsync($"{item.Key}={item.Value}");

                    await writer.FlushAsync();
                });
        
        public void SaveAsXml(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var xml = this.ToXmlString();
            stream.WriteString(xml);
        }

        public void SaveAsXml(string path)
        {
            using (var stream = new StreamWriter(path))
                SaveAsXml(stream);
        }

        public void SaveAsXml(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            
            var xml = this.ToXmlString();

            writer.Write(xml);
            writer.Flush();
        }

        public Task SaveAsXmlAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : Task.Run(async () =>
                {
                    var xml = this.ToXmlString();

                    await stream.WriteStringAsync(xml);
                });

        public Task SaveAsXmlAsync(string path)
            => path == null
                ? throw new ArgumentNullException(nameof(path))
                : Task.Run(async () => 
                {
                    using (var stream = new StreamWriter(path))
                        await SaveAsXmlAsync(stream);
                });

        public Task SaveAsXmlAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    var xml = this.ToXmlString();
            
                    await writer.WriteAsync(xml);
                    await writer.FlushAsync();
                });

        public void SaveAsJson(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var json = this.ToJsonString();
            stream.WriteString(json);
        }            

        public void SaveAsJson(string path)
        {
            using (var stream = new StreamWriter(path))
                SaveAsJson(stream);
        }

        public void SaveAsJson(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            var json = this.ToJsonString();

            writer.Write(json);
            writer.Flush();
        }
        
        public Task SaveAsJsonAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : Task.Run(async () =>
                {
                    var json = this.ToJsonString();
                    await stream.WriteStringAsync(json);
                });

        public Task SaveAsJsonAsync(string path)
            => path == null
                ? throw new ArgumentNullException(nameof(path))
                : Task.Run(async () => 
                {
                    using (var stream = new StreamWriter(path))
                        await SaveAsJsonAsync(stream);
                });

        public Task SaveAsJsonAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    var json = this.ToJsonString();

                    await writer.WriteAsync(json);
                    await writer.FlushAsync();
                });

        public void SaveAsCsv(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var csv = this.ToCsvString();
            stream.WriteString(csv);
        }

        public void SaveAsCsv(string path)
        {
            using (var stream = new StreamWriter(path))
                SaveAsCsv(stream);
        }

        public void SaveAsCsv(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            var csv = this.ToCsvString();

            writer.Write(csv);
            writer.Flush();
        }
        
        public Task SaveAsCsvAsync(Stream stream)
            => stream == null
                ? throw new ArgumentNullException(nameof(stream))
                : Task.Run(async () =>
                {
                    var csv = ToCsvString();
                    await stream.WriteStringAsync(csv);
                });

        public Task SaveAsCsvAsync(string path)
            => path == null
                ? throw new ArgumentNullException(nameof(path))
                : Task.Run(async () => 
                {
                    using (var stream = new StreamWriter(path))
                        await SaveAsCsvAsync(stream);
                });

        public Task SaveAsCsvAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    var csv = this.ToCsvString();

                    await writer.WriteAsync(csv);
                    await writer.FlushAsync();
                });
        
        private string ToXmlString() 
            => _properties.Aggregate(new StringBuilder().AppendLine("<?xml version='1.0' encoding='UTF-8'?>")
                                                        .AppendLine("<properties>"),
                                    (stringBuilder, property) => stringBuilder.AppendLine($"\t<property key='{property.Key}' value='{property.Value}' />"),
                                    stringBuilder => stringBuilder.Append("</properties>").ToString());

        private string ToJsonString()
        {
            var stringBuilder = new StringBuilder().AppendLine("[");

            using (var enumerator = GetEnumerator()) {
                var last = !enumerator.MoveNext();
                KeyValuePair<string, string> current;

                while (!last) {
                    current = enumerator.Current;
                    last = !enumerator.MoveNext();

                    stringBuilder.AppendLine("\t{")
                                 .Append("\t\t\"key\": ").AppendLine($"\"{current.Key}\",")
                                 .Append("\t\t\"value\": ").AppendLine($"\"{current.Value}\"")
                                 .Append("\t}");

                    if (!last)
                     stringBuilder.AppendLine(",");
                }
            }

            return stringBuilder.AppendLine().AppendLine("]").ToString();
        }

        private string ToCsvString()
            => _properties.Aggregate(new StringBuilder(), 
                                    (stringBuilder, property) => stringBuilder.AppendLine($"{property.Key}{Delimiter}{property.Value}"),
                                    stringBuilder => stringBuilder.ToString());
    }
}