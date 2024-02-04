using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class HtmlFormatterTests
    {
        [Test]
        [TestCase("hello world!")]
        public void FormatAsBold_WhenCalled_ShouldEncloseContentWithStrongElements(string content)
        {
            var htmlFormatter = new HtmlFormatter();
            var formatAsBold = htmlFormatter.FormatAsBold(content);
            
            Assert.That(formatAsBold, Is.EqualTo($"<strong>{content}</strong>"));
            
            // Assert.That(formatAsBold, Does.StartWith("<strong>"));
            // Assert.That(formatAsBold, Does.EndWith("</strong>"));
            // Assert.That(formatAsBold, Does.Contain(content));
        }
    }
}