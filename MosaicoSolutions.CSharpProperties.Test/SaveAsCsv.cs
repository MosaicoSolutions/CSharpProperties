using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class SaveAsCsv
    {
        [Fact]
        public void SavePropertiesAsCsv()
        {
            const string csv = 
 @"email;saito@mail.com
role;turist";

            var properties = Properties.LoadFromCsv(new StringReader(csv));

            using(var stringWriter = new StringWriter())
            {
                properties.SaveAsCsv(stringWriter);

                var properties2 = Properties.LoadFromCsv(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2["email"], properties["email"]);
                Assert.Equal(properties2["role"], properties["role"]);
                Assert.Equal(properties2.Count(), properties.Count());
            }
        }

        [Fact]
        public void SavePropertiesAsCsvFile()
        {
            const string csv = 
@"email;saito@mail.com
role;turist";

            var properties = Properties.LoadFromCsv(new StringReader(csv));

            var file = $"{TestDirectory.PropertiesDirectoryPath}/properties2.csv";
            properties.SaveAsCsv(file);

            var properties2 = Properties.LoadFromCsv(file);
            
            Assert.Equal(properties2["role"], properties["role"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }

        [Fact]
        public void PropertiesMustBeEmpty()
        {
            var content = @"";

            var properties = Properties.Load(new StringReader(content));

            using (var stringWriter = new StringWriter())
            {
                properties.SaveAsCsv(stringWriter);

                var properties2 = Properties.LoadFromCsv(new StringReader(stringWriter.ToString()));

                Assert.Equal(properties2.Count(), 0);
                Assert.Empty(properties2);
            }
        }

        [Fact]
        public void MustSaveAndNotDisposeStream()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = Properties.Load($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

                properties.SaveAsCsv(writer);
                
                writer.WriteLine("prop2;value2");
                writer.Flush();

                Assert.Contains("prop2;value2", writer.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        [Fact]
        public async Task MustSaveAndNotDisposeStreamAsync()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = await Properties.LoadAsync($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

                await properties.SaveAsCsvAsync(writer);
                
                await writer.WriteLineAsync("prop2;value2");
                await writer.FlushAsync();

                Assert.Contains("prop2;value2", writer.ToString());
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}