using System.Threading.Tasks;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class PropertiesStrategyTest
    {
        [Fact]
        public void LoadFromJsonString()
        {
            const string json = 
@"[
    {
        'key': 'host', 'value':'localhost'
    },
    {
        'key': 'database', 'value':'inception'
    }
]";
            var properties = Properties.LoadFromJsonString(json);

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["database"], "inception");
            Assert.NotEmpty(properties);
        }

        [Fact]
        public async Task LoadFromJsonStringAsync()
        {
            const string json = 
@"[
    {
        'key': 'host', 'value':'localhost'
    },
    {
        'key': 'database', 'value':'inception'
    }
]";
            var properties = await Properties.LoadFromJsonStringAsync(json);

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["database"], "inception");
            Assert.NotEmpty(properties);
        }

        [Fact]
        public void LoadFromXmlString()
        {
            const string xml = 
@"<?xml version='1.0' encoding='UTF-8'?>
<properties>
    <property key='email' value='ariadne@gmail.com'/>
</properties>";

            var properties = Properties.LoadFromXmlString(xml);

            Assert.Equal(properties["email"], "ariadne@gmail.com");
        }

        [Fact]
        public async Task LoadFromXmlStringAsync()
        {
            const string xml = 
@"<?xml version='1.0' encoding='UTF-8'?>
<properties>
    <property key='email' value='ariadne@gmail.com'/>
</properties>";

            var properties = await Properties.LoadFromXmlStringAsync(xml);

            Assert.Equal(properties["email"], "ariadne@gmail.com");
        }

        [Fact]
        public void LoadFromCsvlString()
        {
            const string csv = 
@"email;saito@mail.com
role;turist";

            var properties = Properties.LoadFromCsvString(csv);

            Assert.Equal(properties["email"], "saito@mail.com");
            Assert.Equal(properties["role"], "turist");
        }

        [Fact]
        public async Task LoadFromCsvStringAsync()
        {
            const string csv = 
@"email;saito@mail.com
role;turist";

            var properties = await Properties.LoadFromCsvStringAsync(csv);

            Assert.Equal(properties["email"], "saito@mail.com");
            Assert.Equal(properties["role"], "turist");
        }
    }
}