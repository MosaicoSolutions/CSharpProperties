using System;
using System.IO;

namespace MosaicoSolutions.CSharpProperties.Test.IO
{
    public static class TestDirectory
    {
        public static string PropertiesDirectoryPath { get; }

        static TestDirectory() 
            => PropertiesDirectoryPath = Path.Combine(Environment.CurrentDirectory, "../../../properties_files");
    }
}