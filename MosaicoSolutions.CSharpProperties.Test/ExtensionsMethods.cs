using Xunit;
using MosaicoSolutions.CSharpProperties.Extensions;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class ExtensionsMethods
    {
            const string content = 
@"user=cobb
password=Inception,isItPossible?";

        [Fact]
        public void GetOrEmpty()
        {
            var properties = Properties.LoadFromString(content);

            var email = properties.GetOrEmpty("email");

            Assert.Equal(email, string.Empty);
        }

        [Fact]
        public void GetOrElse()
        {
            var properties = Properties.LoadFromString(content);
            var emailNotSupplied = "E-mail not supplied";

            var email = properties.GetOrElse("email", emailNotSupplied);

            Assert.Equal(email, emailNotSupplied);
        }
    }
}