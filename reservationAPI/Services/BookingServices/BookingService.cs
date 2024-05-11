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

        //Метод для бронювання житла
        public async Task<ServiceResponse<BookApartmentDto>> Booking(int apartmentId, int userId, BookApartmentDto bookDto)
        {  
            var response = new ServiceResponse<BookApartmentDto>();
            Booking booking = new Booking();

            var apartment = await _dataContext.Apartments.FindAsync(apartmentId);

            //Перевіряємо на null
            if (bookDto is null)
            {
                response.Success = false;
                response.Message = "Something went wrong";
                return response;
            }
            //Перевірка на коректність введених даних
            else if (bookDto.Entry >= bookDto.Leave || bookDto.Leave <= bookDto.Entry)
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

            //Перевіряємо чи житло вже заброньоване
            var existingBooking = await _dataContext.Booking
                                            .AnyAsync(b => b.ApartmentId == apartmentId &&
                                                                b.Entry < bookDto.Leave &&
                                                                    b.Leave > bookDto.Entry);
            //Перевіряємо чи житло вже заброньоване
            if (existingBooking)
            {
                response.Success = false;
                response.Message = "Apartment is already booked for these dates";
                return response; 
            }

            //Додаємо до БД
            booking.ApartmentId = apartmentId;
            booking.UserId = userId;
            booking.Guests = bookDto.Guests;
            booking.Entry = bookDto.Entry;
            booking.Leave = bookDto.Leave;
            booking.TotalPrice = (decimal)(bookDto.Leave - bookDto.Entry).TotalDays * apartment.PricePerDay;


            await _dataContext.AddAsync(booking);
            await _dataContext.SaveChangesAsync();

            //Відправка відповіді
            response.Data = bookDto;

            return response;
        }

        //Метод для відміни бронювання
        public async Task<ServiceResponse<string>> CancelBooking(int id, int userId)
        {
            var response = new ServiceResponse<string>();

            //Знаходимо потрібне нам бронювання
            var deleteBook = await _dataContext.Booking
                                                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            //Перевірка на null
            if (deleteBook is null)
            {
                response.Success = false;
                response.Message = $"Wrong id: {id}";
                return response;
            }

            //Видалення з БД
            _dataContext.Remove(deleteBook);
            await _dataContext.SaveChangesAsync();

            //Відправка відповіді
            response.Data = "Booking canceled";

            return response;

        }

        //Метод для редагування броні
        public async Task<ServiceResponse<BookingEditDto>> EditBooking(int id, int userId, BookingEditDto editDto)
        {
            var response = new ServiceResponse<BookingEditDto>();

            //Знаходимо бронь на зміну
            var editBooking = await _dataContext.Booking
                                                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            //Знаходимо заброньоване житло
            var apartment = await _dataContext.Apartments.FirstOrDefaultAsync(x=>x.Id == editBooking.ApartmentId);

            //Перевірка на введення та null
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

            //Вносимо зміни до БД
            editBooking.Entry = editDto.Entry;
            editBooking.Leave = editDto.Leave;  
            editBooking.Guests = editDto.Guests;

            _dataContext.Update(editBooking);
            await _dataContext.SaveChangesAsync();

            //Відправка відповіді
            response.Data = editDto;
            return response;

        }

        //Метод для перегляду бронювання клієнта
        public async Task<ServiceResponse<List<BookInfoDto>>> MyBookings(int userId)
        {
            var response = new ServiceResponse<List<BookInfoDto>>();

            //Шукаємо всі бронювання клієнта
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

            //Перевірка на null
            if (books is null)
            {
                response.Success = false;
                response.Message = "Your booking list is empty";
                return response;
            }

            //Відправка відповіді
            response.Data = books;

            return response;

        }
    }
}
