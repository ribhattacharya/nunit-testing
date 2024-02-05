#nullable enable
using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(Booking? booking = null);
    }

    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetActiveBookings(Booking? booking = null)
        {
            var unitOfWork = new UnitOfWork();
            var bookings =
                unitOfWork.Query<Booking>()
                    .Where(b => b.Status != "Cancelled");

            if (booking != null)
                bookings = bookings.Where(b => b.Id != booking.Id);
            
            return bookings;
        }
    }
}