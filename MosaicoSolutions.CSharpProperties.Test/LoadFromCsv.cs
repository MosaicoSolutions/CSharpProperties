using System.IO;
using System.Linq;
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
    }
}