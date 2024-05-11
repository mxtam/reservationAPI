using Microsoft.EntityFrameworkCore;
using reservationAPI.Data;
using reservationAPI.Dtos.Booking;
using reservationAPI.Migrations;
using reservationAPI.Models;

namespace reservationAPI.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly DataContext _dataContext;

        public BookingService(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<BookApartmentDto>> Booking(int apartmentId, int userId, BookApartmentDto bookDto)
        {
            var response = new ServiceResponse<BookApartmentDto>();
            Booking booking = new Booking();

            var apartment = await _dataContext.Apartments.FindAsync(apartmentId);

            if (bookDto.Entry >= bookDto.Leave || bookDto.Leave <= bookDto.Entry)
            {
                response.Success = false;
                response.Message = "Incorrect date";
                return response;
            }
            else if (bookDto.Guests > apartment.MaxGuests || bookDto.Guests <= 0) 
            {
                response.Success = false;
                response.Message = "Incorrect count of guests";
                return response;
            }

            var existingBooking = await _dataContext.Booking
                                            .AnyAsync(b => b.ApartmentId == apartmentId &&
                                                                b.Entry < bookDto.Leave &&
                                                                    b.Leave > bookDto.Entry);

            if (existingBooking)
            {
                response.Success = false;
                response.Message = "Apartment is already booked for these dates";
                return response; 
            }

            booking.ApartmentId = apartmentId;
            booking.UserId = userId;
            booking.Guests = bookDto.Guests;
            booking.Entry = bookDto.Entry;
            booking.Leave = bookDto.Leave;
            booking.TotalPrice = (decimal)(bookDto.Leave - bookDto.Entry).TotalDays * apartment.PricePerDay;


            await _dataContext.AddAsync(booking);
            await _dataContext.SaveChangesAsync();

            if (bookDto is null)
            {
                response.Success = false;
                response.Message = "Something went wrong";
                return response;
            }

            response.Data = bookDto;

            return response;
        }

        public async Task<ServiceResponse<string>> CancelBooking(int id, int userId)
        {
            var response = new ServiceResponse<string>();

            var deleteBook = await _dataContext.Booking
                                                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (deleteBook is null)
            {
                response.Success = false;
                response.Message = $"Wrong id: {id}";
                return response;
            }

            _dataContext.Remove(deleteBook);
            await _dataContext.SaveChangesAsync();

            response.Data = "Booking canceled";

            return response;

        }

        public async Task<ServiceResponse<BookingEditDto>> EditBooking(int id, int userId, BookingEditDto editDto)
        {
            var response = new ServiceResponse<BookingEditDto>();

            var editBooking = await _dataContext.Booking
                                                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            var apartment = await _dataContext.Apartments.FirstOrDefaultAsync(x=>x.Id == editBooking.ApartmentId);

            if (editBooking is null)
            { 
                response.Success=false;
                response.Message = $"Can`t find booking with id: {id}";

                return response;
            }
            else if (editDto.Entry >= editDto.Leave || editDto.Leave <= editDto.Entry)
            {
                response.Success = false;
                response.Message = "Incorrect date";
                return response;
            }
            else if (editDto.Guests > apartment.MaxGuests || editDto.Guests <= 0)
            {
                response.Success = false;
                response.Message = "Incorrect count of guests";
                return response;
            }

            var existingBooking = await _dataContext.Booking
                                            .AnyAsync(b => b.ApartmentId == editBooking.ApartmentId &&
                                                                b.Entry < editDto.Leave &&
                                                                    b.Leave > editDto.Entry);

            if (existingBooking)
            {
                response.Success = false;
                response.Message = "Apartment is already booked for these dates";
                return response;
            }

            editBooking.Entry = editDto.Entry;
            editBooking.Leave = editDto.Leave;  
            editBooking.Guests = editDto.Guests;

            _dataContext.Update(editBooking);
            await _dataContext.SaveChangesAsync();

            response.Data = editDto;
            return response;

        }

        public async Task<ServiceResponse<List<BookInfoDto>>> MyBookings(int userId)
        {
            var response = new ServiceResponse<List<BookInfoDto>>();

            var books = await _dataContext.Booking.Where(x => x.UserId == userId).Select(b => new BookInfoDto
            {
                Id = b.Id,
                ApartmentName = b.Apartment.Name,
                ApartmentLocation = b.Apartment.Location,
                Entry = b.Entry,
                Leave = b.Leave,
                Guests = b.Guests,
                TotalPrice = (decimal)(b.Leave - b.Entry).TotalDays * b.Apartment.PricePerDay
            }).ToListAsync();

            if (books is null)
            {
                response.Success = false;
                response.Message = "Your booking list is empty";
            }
            response.Data = books;

            return response;

        }
    }
}
