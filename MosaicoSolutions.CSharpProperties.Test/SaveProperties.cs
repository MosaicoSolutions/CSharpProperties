using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class SaveProperties
    {
        [Fact]
        public void SaveInFile()
        {
            var destFile = @"C:\temp\userProperties.properties";

            var content = @"
            # User Properties
            username=cooper
            passwrod=star
            ";
    
            var properties = Properties.Load(new StringReader(content));

            properties.Add("email", "cooper@mail.com");
            properties.Save(destFile);

            IProperties userProperties;

            using(var reader = new StreamReader(destFile))
                userProperties = Properties.Load(destFile);

            Assert.Equal(userProperties["email"], "cooper@mail.com");
            Assert.Equal(userProperties.Count(), 3);
        }

        [Fact]
        public async Task SaveInFileAsync()
        {
            var file = @"C:\temp\db.properties";

            var properties = await Properties.LoadAsync(new FileStream(file, FileMode.Open));

            properties.Add("sgbd", "MySql");

            await properties.SaveAsync(@"C:\temp\newDb.properties");

            var dbProperties = await Properties.LoadAsync(@"C:\temp\newDb.properties");

            Assert.Equal(properties["sgbd"], "MySql");
        }
    }
}