using System.IO;
using System.Linq;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class LoadFromCsv
    {
        [Fact]
        public void LoadFromString()
        {
            const string csv = 
@"email;saito@mail.com
role;turist";

            var properties = Properties.LoadFromCsv(new StringReader(csv));

            Assert.Equal(properties.Count(), 2);
            Assert.Equal(properties["email"], "saito@mail.com");
            Assert.Equal(properties.Get("role"), "turist");
        }

        [Fact]
        public void MustLoadAndDoNotDisposeStream()
        {
            string sourceFile = $"{TestDirectory.PropertiesDirectoryPath}/properties.csv";
            string targetFile = $"{TestDirectory.PropertiesDirectoryPath}/properties_copy.csv";

            File.Copy(sourceFileName: sourceFile, destFileName: targetFile, overwrite: true);

            using (var stream = File.Open(targetFile, FileMode.Open))
            {
                var properties = Properties.LoadFromCsv(stream);

                stream.WriteByte(byte.MinValue);
                stream.WriteByte(byte.MaxValue);
            }
        }
    }
}