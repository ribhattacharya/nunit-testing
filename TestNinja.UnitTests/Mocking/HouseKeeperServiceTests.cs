using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private Housekeeper _housekeeper;
        private HouseKeeperService _houseKeeperService;
        private readonly DateTime _statementDate = DateTime.Today;
        private string _statementFileName;

        [SetUp]
        public void SetUp()
        {
            _housekeeper = new Housekeeper
            {
                Email = "hello@google.com",
                FullName = "John Smith",
                Oid = 1,
                StatementEmailBody = "Your statement is here."
            };
            
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(
                new List<Housekeeper> { _housekeeper }.AsQueryable());

            _statementFileName = "statement.csv";
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator
                .Setup(sg => 
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(() => _statementFileName); // Func overload for Returns, for live changes to _statementFileName 
            
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            
            _houseKeeperService = new HouseKeeperService(
                _unitOfWork.Object,
                _statementGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {
            _houseKeeperService.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_WhenCalledWithInvalidEmail_DoesNotGenerateStatements(string email)
        {
            _housekeeper.Email = email;
            
            _houseKeeperService.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => 
                sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_EmailStatements()
        {
            _houseKeeperService.SendStatementEmails(_statementDate);

            VerifyEmailSent();
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_StatementFilenameIsInvalid_DoesNotEmailStatements(string filename)
        {
            _statementFileName = filename;
            
            _houseKeeperService.SendStatementEmails(_statementDate);
            
            VerifyEmailNotSent();
        }
        
        [Test]
        public void SendStatementEmails_EmailSendingFails_DisplayMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsNotNull<string>(),
                It.IsNotNull<string>(),
                It.IsNotNull<string>(),
                It.IsNotNull<string>())).Throws<Exception>();
            
            _houseKeeperService.SendStatementEmails(_statementDate);

            VerifyMessageBoxIsDisplayed();
        }

        private void VerifyMessageBoxIsDisplayed()
        {
            _messageBox.Verify(mb => mb.Show(
                It.IsNotNull<string>(),
                It.IsNotNull<string>(),
                MessageBoxButtons.Ok));
        }

        private void VerifyEmailSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                It.IsNotNull<string>(), 
                It.IsNotNull<string>(),
                It.IsNotNull<string>(),
                It.IsNotNull<string>()));
        }

        private void VerifyEmailNotSent()
        {
            _emailSender.Verify(es => es.EmailFile(
                It.IsNotNull<string>(), 
                It.IsNotNull<string>(),
                It.IsNotNull<string>(),
                It.IsNotNull<string>()), Times.Never);
        }
    }
}