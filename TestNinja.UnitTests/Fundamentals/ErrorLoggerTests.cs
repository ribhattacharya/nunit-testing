using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class ErrorLoggerTests
    {
        [Test]
        public void Log_ValidError_LogsError()
        {
            const string error = "hello world";
            var errorLogger = new ErrorLogger();
            
            errorLogger.Log(error);
            
            Assert.That(errorLogger.LastError, Is.EqualTo(error));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Log_InvalidError_ThrowsException(string error)
        {
            var errorLogger = new ErrorLogger();
            
            Assert.That(() => errorLogger.Log(error), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Log_ValidError_LogsEvent()
        {
            var errorLogger = new ErrorLogger();
            var id = Guid.Empty;

            errorLogger.ErrorLogged += (sender, args) => { id = args; }; 
            
            errorLogger.Log("a");
            
            Assert.That(id, Is.Not.EqualTo(Guid.Empty));
        }
    }
}