using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelperOverlappingBookingsExistTests
    {
        private Booking _existingBooking, _newBooking;
        private Mock<IBookingRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                Id = 2,
                Reference = "existing booking",
                Status = "Not cancelled",
                ArrivalDate = ArriveOn(2024, 10, 14),
                DepartureDate = DepartOn(2024, 10, 20),
            };
            _newBooking = new Booking
            {
                Id = 1, 
                Reference = "new booking", 
                Status = "Not cancelled"
            };
            
            _repository = new Mock<IBookingRepository>();
            _repository.Setup(rp => rp.GetActiveBookings(_newBooking)).
                Returns(new List<Booking> { _existingBooking }.AsQueryable());
        }

        [Test]
        public void BookingStartsAndEndsBeforeAnotherBooking_ReturnsEmptyString()
        {
            _newBooking.ArrivalDate = DaysBefore(_existingBooking.ArrivalDate, 5);
            _newBooking.DepartureDate = DaysBefore(_existingBooking.ArrivalDate, 2);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void BookingStartsBeforeAndEndsInBetweenAnotherBooking_ReturnsReference()
        {
            _newBooking.ArrivalDate = DaysBefore(_existingBooking.ArrivalDate, 5);
            _newBooking.DepartureDate = DaysAfter(_existingBooking.ArrivalDate, 1);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        
        [Test]
        public void BookingStartsBeforeAndEndsAfterAnotherBooking_ReturnsReference()
        {
            _newBooking.ArrivalDate = DaysBefore(_existingBooking.ArrivalDate, 5);
            _newBooking.DepartureDate = DaysAfter(_existingBooking.DepartureDate, 1);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        
        [Test]
        public void BookingStartsAndEndsInTheMiddleOfAnotherBooking_ReturnsReference()
        {
            _newBooking.ArrivalDate = DaysAfter(_existingBooking.ArrivalDate, 1);
            _newBooking.DepartureDate = DaysBefore(_existingBooking.DepartureDate, 1);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        
        [Test]
        public void BookingStartsInTheMiddleOfAnotherBookingAndEndsAfter_ReturnsReference()
        {
            _newBooking.ArrivalDate = DaysAfter(_existingBooking.ArrivalDate, 1);
            _newBooking.DepartureDate = DaysAfter(_existingBooking.DepartureDate, 1);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }
        
        [Test]
        public void BookingStartsAndEndsAfterAnotherBooking_ReturnsEmptyString()
        {
            _newBooking.ArrivalDate = DaysAfter(_existingBooking.DepartureDate, 1);
            _newBooking.DepartureDate = DaysAfter(_existingBooking.DepartureDate, 5);

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(string.Empty));
        }
        
        [Test]
        public void BookingOverlapsButNewBookingIsCancelled_ReturnsEmptyString()
        {
            _newBooking.ArrivalDate = DaysAfter(_existingBooking.ArrivalDate, 1);
            _newBooking.DepartureDate = DaysAfter(_existingBooking.DepartureDate, 1);
            _newBooking.Status = "Cancelled";

            var result = BookingHelper.OverlappingBookingsExist(_newBooking, _repository.Object);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        private static DateTime ArriveOn(int year, int month, int day)
        {
            // Arrivals usually at 2 pm
            return new DateTime(year, month, day, 12, 0, 0);
        }
        
        private static DateTime DepartOn(int year, int month, int day)
        {
            // Arrivals usually at 10 am
            return new DateTime(year, month, day, 10, 0, 0);
        }

        private static DateTime DaysBefore(DateTime dateTime, int days = 0)
        {
            return dateTime.AddDays(-days);
        }
        
        private static DateTime DaysAfter(DateTime dateTime, int days = 0)
        {
            return dateTime.AddDays(days);
        }
    }
}