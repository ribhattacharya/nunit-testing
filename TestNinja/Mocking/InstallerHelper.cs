using System.Net;

namespace TestNinja.Mocking
{
    public class InstallerHelper
    {
        private readonly IFileDownloader _fileDownloader; 
        private string _setupDestinationFile;

        public InstallerHelper(IFileDownloader fileDownloader = null)
        {
            _fileDownloader = fileDownloader ?? new FileDownloader();
        }

        public bool DownloadInstaller(string customerName, string installerName)
        {
            try
            {
                var url = $"https://example.com/{customerName}/{installerName}";
                _fileDownloader.DownloadFile(url, _setupDestinationFile);
                
                return true;
            }
            catch (WebException)
            {
                return false; 
            }
        }
    }
}