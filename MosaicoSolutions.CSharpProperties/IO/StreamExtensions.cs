using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MosaicoSolutions.CSharpProperties.IO
{
    public static class StreamExtensions
    {
        public static void WriteProperties(this Stream @this, IProperties properties) 
            => properties.Select(property => $"{property.Key}={property.Value}{Environment.NewLine}")
                         .ToList()
                         .ForEach(property => @this.WriteString(property));

        public static Task WritePropertiesAsync(this Stream @this, IProperties properties)
            => WritePropertiesAsync(@this, properties, CancellationToken.None);

        public static Task WritePropertiesAsync(this Stream @this, IProperties properties, CancellationToken cancellationToken)
            => Task.Run(() => properties.Select(property => $"{property.Key}={property.Value}{Environment.NewLine}")
                                        .ToList()
                                        .ForEach(async property => await @this.WriteStringAsync(property, cancellationToken)));

        public static void WriteString(this Stream @this, string value)
        {
            var bytes = GetBytes(value);
            @this.Write(bytes, 0, bytes.Length);
            @this.Flush();
        }

        public static Task WriteStringAsync(this Stream @this, string value)
            => WriteStringAsync(@this, value, CancellationToken.None);

        public static Task WriteStringAsync(this Stream @this, string value, CancellationToken cancellationToken)
            => Task.Run(async () => 
            {
                var bytes = GetBytes(value);
                await @this.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
                await @this.FlushAsync();
            });

        private static byte[] GetBytes(string property)
            => Encoding.UTF8.GetBytes(property);
    }
}