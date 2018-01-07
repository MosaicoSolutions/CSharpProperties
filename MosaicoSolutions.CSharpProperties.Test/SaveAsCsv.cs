using System.IO;
using System.Linq;
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
        public void SavePropertiesAsJsonFile()
        {
            const string csv = 
@"email;saito@mail.com
role;turist";

            var properties = Properties.LoadFromCsv(new StringReader(csv));

            var file = @"C:\temp\properties.csv";
            properties.SaveAsCsv(file);

            var properties2 = Properties.LoadFromCsv(file);
            
            Assert.Equal(properties2["role"], properties["role"]);
            Assert.Equal(properties2.Count(), properties.Count());
        }
    }
}