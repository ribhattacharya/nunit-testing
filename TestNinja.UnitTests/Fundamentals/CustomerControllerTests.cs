using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class CustomerControllerTests
    {
        [Test]
        public void GetCustomer_WhenIDIsZero_NotFound()
        {
            var customerController = new CustomerController();
            var result = customerController.GetCustomer(0);
            
            Assert.That(result, Is.TypeOf<NotFound>());
        }
        
        [Test]
        public void GetCustomer_WhenIDIsNonZero_OK()
        {
            var customerController = new CustomerController();
            var result = customerController.GetCustomer(3);
            
            Assert.That(result, Is.TypeOf<Ok>());
        }
    }
}