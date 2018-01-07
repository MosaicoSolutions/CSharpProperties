using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace MosaicoSolutions.CSharpProperties
{
    public sealed partial class Properties
    {
        public void Save(string path)
            => Save(new StreamWriter(path));

        public void Save(Stream stream)
            => Save(new StreamWriter(stream));
        
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

        public Task SaveAsync(string path)
            => SaveAsync(new StreamWriter(path));

        public Task SaveAsync(Stream stream)
            => SaveAsync(new StreamWriter(stream));
        
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
        
        public void SaveAsXml(string path)
            => SaveAsXml(new StreamWriter(path));

        public void SaveAsXml(Stream stream)
            => SaveAsXml(new StreamWriter(stream));

        public void SaveAsXml(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            
            var xml = ToXmlString();
            
            using (writer)
            {
                writer.Write(xml);
                writer.Flush();
            }
        }

        public Task SaveAsXmlAsync(string path)
            => SaveAsXmlAsync(new StreamWriter(path));

        public Task SaveAsXmlAsync(Stream stream)
            => SaveAsXmlAsync(new StreamWriter(stream));

        public Task SaveAsXmlAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    var xml = ToXmlString();
            
                    using (writer)
                    {
                        await writer.WriteAsync(xml);
                        await writer.FlushAsync();
                    }
                });

        public void SaveAsJson(string path)
            => SaveAsJson(new StreamWriter(path));

        public void SaveAsJson(Stream stream)
            => SaveAsJson(new StreamWriter(stream));

        public void SaveAsJson(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            var json = ToJsonString();

            using(writer)
            {
                writer.Write(json);
                writer.Flush();
            }
        }
        
        public Task SaveAsJsonAsync(string path)
            => SaveAsJsonAsync(new StreamWriter(path));

        public Task SaveAsJsonAsync(Stream stream)
            => SaveAsJsonAsync(new StreamWriter(stream));

        public Task SaveAsJsonAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    var json = ToJsonString();

                    using(writer)
                    {
                        await writer.WriteAsync(json);
                        await writer.FlushAsync();
                    }
                });

        public void SaveAsCsv(string path)
            => SaveAsCsv(new StreamWriter(path));

        public void SaveAsCsv(Stream stream)
            => SaveAsCsv(new StreamWriter(stream));

        public void SaveAsCsv(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            using (writer)
            {
                foreach (var property in _properties)
                    writer.WriteLine($"{property.Key}{Delimiter}{property.Value}");
                 
                writer.Flush();
            }
        }
        
        public Task SaveAsCsvAsync(string path)
            => SaveAsCsvAsync(new StreamWriter(path));

        public Task SaveAsCsvAsync(Stream stream)
            => SaveAsCsvAsync(new StreamWriter(stream));

        public Task SaveAsCsvAsync(TextWriter writer)
            => writer == null
                ? throw new ArgumentNullException(nameof(writer))
                : Task.Run(async () =>
                {
                    using (writer)
                    {
                        foreach (var property in _properties)
                            await writer.WriteLineAsync($"{property.Key}{Delimiter}{property.Value}");

                        await writer.FlushAsync();
                    }
                });
        
        public string ToXmlString() 
            => _properties.Aggregate(new StringBuilder().AppendLine("<?xml version='1.0' encoding='UTF-8'?>")
                                                        .AppendLine("<properties>"),
            (stringBuilder, property) => stringBuilder.AppendLine($"\t<property key='{property.Key}' value='{property.Value}' />"),
            stringBuilder => stringBuilder.Append("</properties>").ToString());
        
        public string ToJsonString()
            => _properties.Aggregate(new StringBuilder().AppendLine("["), 
                (stringBuilder, property) => 
                    stringBuilder.AppendLine("\t{")
                        .Append("\t\t\"key\": ").AppendLine($"\"{property.Key}\",")
                        .Append("\t\t\"value\": ").AppendLine($"\"{property.Value}\"")
                        .AppendLine("\t},"),
                stringBuilder => stringBuilder.Remove(stringBuilder.Length - 3, 3) // To remove the last ',' 
                    .AppendLine().Append("]").ToString());
    }
}