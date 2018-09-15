using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MosaicoSolutions.CSharpProperties.Test.IO;
using Xunit;

namespace MosaicoSolutions.CSharpProperties.Test
{
    public class LoadProperties
    {
        private readonly string _content = @"
        # This is a comment
        host=localhost
        port=123
        database=test
        ";

        [Fact]
        public void LoadFromStringReader()
        {
            var properties = Properties.Load(new StringReader(_content));

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["port"], "123");
            Assert.Equal(properties["database"], "test");

            Assert.Equal(properties.Count(), 3);
        }

        [Fact]
        public async Task LoadFromStringReaderAsync()
        {
            var properties = await Properties.LoadAsync(new StringReader(_content));

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["port"], "123");
            Assert.Equal(properties["database"], "test");

            Assert.Equal(properties.Count(), 3);
        }

        public async Task LoadFromFileAsync()
        {
            var properties = await Properties.LoadAsync($"{TestDirectory.PropertiesDirectoryPath}/db.properties");

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["port"], "8080");
            Assert.Equal(properties["database"], "test");

            Assert.Equal(properties.Count(), 3);
        }

        [Fact]
        public void LoadWithHandles()
        {
            var content = @"
            username:=cooper
            
            #Don't make this!
            password:=1234

            #Will not be captured by the handle
            email=cooper@mail
            ";

            bool IsValidLine(string line) =>
                !line.StartsWith("#") && line.Contains(":=");

            KeyValuePair<string, string> PropertyHandle(string line)
            {
                var tokens = line.Split(":=");
                return tokens.Length == 2
                        ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
                        : new KeyValuePair<string, string>();
            }
        
            var properties = PropertiesBuild.NewPropertiesBuild()
                                            .WithValidLineHandle(IsValidLine)
                                            .WithExtractPropertyHandle(PropertyHandle)
                                            .BuildWithReader(new StringReader(content));

            Assert.Equal(properties["password"], "1234");
            Assert.Equal(properties["username"], "cooper");
            Assert.Equal(properties.Count(), 2);
        }

        [Fact]
        public async Task LoadWithHandlesAsync()
        {
            var content = @"
            username:=cooper
            
            #Don't make this!
            password:=1234

            #Will not be captured by the handle
            email=cooper@mail
            ";

            bool IsValidLine(string line) =>
                !line.StartsWith("#") && line.Contains(":=");

            KeyValuePair<string, string> PropertyHandle(string line)
            {
                var tokens = line.Split(":=");
                return tokens.Length == 2
                        ? new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim())
                        : new KeyValuePair<string, string>();
            }
        
            var properties = await PropertiesBuild.NewPropertiesBuild()
                                                  .WithValidLineHandle(IsValidLine)
                                                  .WithExtractPropertyHandle(PropertyHandle)
                                                  .BuildWithReaderAsync(new StringReader(content));

            Assert.Equal(properties["password"], "1234");
            Assert.Equal(properties["username"], "cooper");
            Assert.Equal(properties.Count(), 2);
        }

        [Fact]
        public void LoadFromFileAsyncMustThrowException()
            => Assert.Throws<ArgumentNullException>(() => 
            {
                var properties = Properties.LoadAsync((string)null);
            });

        [Fact]
        public void MustLoadAndDoNotDisposeStream()
        {
            using (var stream = File.Open($"{TestDirectory.PropertiesDirectoryPath}/properties.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var properties = Properties.Load(stream);

                stream.WriteByte(5);
                stream.Flush();
            }
        }

        [Fact]
        public void LoadFromString()
        {
            var properties = Properties.LoadFromString(_content);

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["port"], "123");
            Assert.Equal(properties["database"], "test");

            Assert.Equal(properties.Count(), 3);
        }

        [Fact]
        public async Task LoadFromStringAsync()
        {
            var properties = await Properties.LoadFromStringAsync(_content);

            Assert.Equal(properties["host"], "localhost");
            Assert.Equal(properties["port"], "123");
            Assert.Equal(properties["database"], "test");

            Assert.Equal(properties.Count(), 3);
        }
    }
}