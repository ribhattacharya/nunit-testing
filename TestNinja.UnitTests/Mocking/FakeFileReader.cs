using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    public class ValidFakeFileReader : IFileReader
    {
        public string Read(string path)
        {
            return "fake file reader";
        }
    }
    
    public class InValidFakeFileReader : IFileReader
    {
        public string Read(string path)
        {
            return "";
        }
    }
}