using Microsoft.EntityFrameworkCore;
using reservationAPI.Data;
using reservationAPI.Dtos.Apartment;
using reservationAPI.Models;
using System.Globalization;

namespace reservationAPI.Services.ApartmentServices
{
    public class ApartmentService : IApartmentService
    {
        private readonly DataContext _dataContext;
        CultureInfo uaCulture = new CultureInfo("uk-UA");

        public ApartmentService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //Метод для перегляду доступного для бронювання житла
        public async Task<ServiceResponse<List<ApartmentGetDto>>> GetAll()
        {
            var response = new ServiceResponse<List<ApartmentGetDto>>();

            //Знаходимо доступне житло з БД
            var apartments = await _dataContext.Apartments.ToListAsync();

            //Перевірка на null
            if (apartments is null)
            {
                response.Success = false;
                response.Message = "Apartments not found";
            }

            //Відправка відповіді 
            response.Data = apartments.Select(a=> new ApartmentGetDto 
                { 
                    Id = a.Id,
                    Name = a.Name,
                    Rooms = a.Rooms,
                    PricePerDay = a.PricePerDay.ToString("C",uaCulture),
                    Location = a.Location,
                    MaxGuests = a.MaxGuests,
                    Description = a.Description

                }).ToList();

            return response;
        }

        //Метод для перегляду жиьла по Id
        public async Task<ServiceResponse<ApartmentGetDto>> GetById(int id)
        {
            var response = new ServiceResponse<ApartmentGetDto>();

            //Знаходимо житло за Id
            var apartment = await _dataContext.Apartments.Select(a=> new ApartmentGetDto 
                {
                Id = a.Id,
                Name = a.Name,
                Rooms = a.Rooms,
                PricePerDay = a.PricePerDay.ToString("C",uaCulture),
                Location = a.Location,
                MaxGuests = a.MaxGuests,
                Description = a.Description

            }).FirstOrDefaultAsync(a => a.Id == id);

            //Перевірка на null
            if (apartment is null)
            {
                response.Success = false;
                response.Message = $"Can`t find apartment with id: {id}";
            }

            //Відправка відповіді
            response.Data = apartment;
            return response;
        }


    }
}
