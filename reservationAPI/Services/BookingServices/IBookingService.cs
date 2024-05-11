using reservationAPI.Dtos.Booking;
using reservationAPI.Models;

namespace reservationAPI.Services.BookingServices
{
    public interface IBookingService
    {
        Task<ServiceResponse<BookApartmentDto>> Booking(int apartmentId, int userId, BookApartmentDto bookDto);
        Task<ServiceResponse<List<BookInfoDto>>> MyBookings(int userId);
        Task<ServiceResponse<string>> CancelBooking(int id, int userId);
        Task<ServiceResponse<BookingEditDto>> EditBooking(int id, int userId, BookingEditDto editDto);
    }
}
