using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class ReservationTests
    {
        private User _adminUser;
        private User _nonAdminUser;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _adminUser = new User { IsAdmin = true };
            _nonAdminUser = new User { IsAdmin = false };
        }
        
        [Test]
        public void CanBeCancelledBy_UserIsAdminAndMadeByAdmin_True()
        {
            var reservation = new Reservation { MadeBy = _adminUser };
            Assert.That(reservation.CanBeCancelledBy(_adminUser), Is.True);
        }
        
        [Test]
        public void CanBeCancelledBy_UserIsAdminAndNotMadeByAdmin_True()
        {
            var reservation = new Reservation { MadeBy = _nonAdminUser };
            Assert.That(reservation.CanBeCancelledBy(_adminUser), Is.True);
        }
        
        [Test]
        public void CanBeCancelledBy_UserIsNonAdminAndMadeByNonAdmin_True()
        {
            var reservation = new Reservation { MadeBy = _nonAdminUser };
            Assert.That(reservation.CanBeCancelledBy(_nonAdminUser), Is.True);
        }
        
        [Test]
        public void CanBeCancelledBy_UserIsNonAdminAndMadeByAdmin_False()
        {
            var reservation = new Reservation { MadeBy = _adminUser };
            Assert.That(reservation.CanBeCancelledBy(_nonAdminUser), Is.False);
        }
    }
}