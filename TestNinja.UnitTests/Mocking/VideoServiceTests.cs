using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepository;
        private VideoService _videoService;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();
            
            _videoService = new VideoService(_fileReader.Object, _videoRepository.Object);
        }
        
        [Test]
        public void ReadVideoTitle_InValidFileReader_ReturnsErrorString()
        {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");
            var title = _videoService.ReadVideoTitle();
            
            Assert.That(title, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnEmptyString()
        {
            _videoRepository.Setup(vr => vr.GetUnprocessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();
            
            Assert.That(result, Is.EqualTo(""));
        }
        
        [Test]
        public void GetUnprocessedVideosAsCsv_SomeVideosAreUnProcessed_ReturnStringWithVideoIds()
        {
            var unProcessedVideosBeingReturned = new List<Video> { 
                new Video { Id = 1 }, 
                new Video { Id = 2 }, 
                new Video { Id = 3 } 
            };
            
            _videoRepository.Setup(vr => vr.GetUnprocessedVideos()).Returns(unProcessedVideosBeingReturned);

            var result = _videoService.GetUnprocessedVideosAsCsv();
            
            Assert.That(result, Is.EqualTo("1,2,3"));
        }
    }
}