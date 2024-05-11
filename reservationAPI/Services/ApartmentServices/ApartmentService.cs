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

        
        public async Task<ServiceResponse<List<ApartmentGetDto>>> GetAll()
        {
            var response = new ServiceResponse<List<ApartmentGetDto>>();
            var apartments = await _dataContext.Apartments.ToListAsync();
            if (apartments is null)
            {
                response.Success = false;
                response.Message = "Apartments not found";
            }

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

        public async Task<ServiceResponse<ApartmentGetDto>> GetById(int id)
        {
            var response = new ServiceResponse<ApartmentGetDto>();
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
            if (apartment is null)
            {
                response.Success = false;
                response.Message = $"Can`t find apartment with id: {id}";
            }
            response.Data = apartment;
            return response;
        }


    }
}
