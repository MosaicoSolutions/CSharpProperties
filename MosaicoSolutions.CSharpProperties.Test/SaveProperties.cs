using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class SaveProperties
    {
        [Fact]
        public void SaveInFile()
        {
            var destFile = $"{TestDirectory.PropertiesDirectoryPath}/userProperties.properties";

            var content = @"
            # User Properties
            username=cooper
            passwrod=star
            ";
    
            var properties = Properties.Load(new StringReader(content));

            properties.Add("email", "cooper@mail.com");
            properties.Save(destFile);

            var userProperties = Properties.Load(destFile);

            Assert.Equal(userProperties["email"], "cooper@mail.com");
            Assert.Equal(userProperties.Count(), 3);
        }

        [Fact]
        public async Task SaveInFileAsync()
        {
            var file = $@"{TestDirectory.PropertiesDirectoryPath}/properties.txt";

            var properties = await Properties.LoadAsync(new FileStream(file, FileMode.Open));

            properties.Add("sgbd", "MySql");

            await properties.SaveAsync($@"{TestDirectory.PropertiesDirectoryPath}/newDb.properties");

            var dbProperties = await Properties.LoadAsync($@"{TestDirectory.PropertiesDirectoryPath}/newDb.properties");

            Assert.Equal(properties["sgbd"], "MySql");
        }

        [Fact]
        public void SaveMustThrowException()
            => Assert.Throws<ArgumentNullException>(() => 
            {
                var file = $"{TestDirectory.PropertiesDirectoryPath}/db.properties";

                var properties = Properties.Load(new FileStream(file, FileMode.Open));

                properties.Add("sgbd2", "Oracle");

                properties.SaveAsync((string)null);
            });

        [Fact]
        public void MustSaveAndNotDisposeStream()
        {
            TextWriter writer = null;

            try
            {
                writer = new StringWriter();
                var properties = Properties.Load($"{TestDirectory.PropertiesDirectoryPath}/db.properties");
                var oldPropertiesCount = properties.Count();

                properties.Add("prop1", "value1");
                properties.Save(writer);
                
                writer.WriteLine("prop2=value2");
                writer.Flush();

                Assert.Contains("prop2=value2", writer.ToString());
                Assert.Equal(properties.Count(), oldPropertiesCount + 1);
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
                var oldPropertiesCount = properties.Count();

                properties.Add("prop1", "value1");
                await properties.SaveAsync(writer);
                
                await writer.WriteLineAsync("prop2=value2");
                await writer.FlushAsync();

                Assert.Contains("prop2=value2", writer.ToString());
                Assert.Equal(properties.Count(), oldPropertiesCount + 1);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}