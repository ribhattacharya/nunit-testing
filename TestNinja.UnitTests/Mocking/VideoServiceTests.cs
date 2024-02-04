using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private VideoService _videoService;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _videoService = new VideoService(_fileReader.Object);
        }
        
        [Test]
        public void ReadVideoTitle_InValidFileReader_ReturnsErrorString()
        {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");
            var title = _videoService.ReadVideoTitle();
            
            Assert.That(title, Does.Contain("error").IgnoreCase);
        }
    }
}