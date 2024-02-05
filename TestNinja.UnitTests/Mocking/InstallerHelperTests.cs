using System.Net;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IFileDownloader> _fileDownloader;
        private InstallerHelper _installerHelper;

        [SetUp]
        public void SetUp()
        {
            _fileDownloader = new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_fileDownloader.Object);
        }
        
        [Test]
        public void DownloadInstaller_DownloadsProperly_ReturnsTrue()
        {
            // No need for _fileDownloader since execution path only diverges on WebException, and true for all else   
            var result = _installerHelper.DownloadInstaller("customer", "installer");
            
            Assert.That(result, Is.True);
        }
        
        [Test]
        public void DownloadInstaller_ThrowsException_ReturnsFalse()
        {
            _fileDownloader.Setup(fd => fd.DownloadFile(It.IsAny<string>(), It.IsAny<string>())).Throws<WebException>();
            // DownloadFile will throw when it is called with the exact same arguments as provided above.
            
            var result = _installerHelper.DownloadInstaller("customer", "installer");
            
            Assert.That(result, Is.False);
        }
    }
}