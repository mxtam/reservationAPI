using reservationAPI.Dtos.Apartment;
using reservationAPI.Models;

namespace reservationAPI.Services.ApartmentServices
{
    public interface IApartmentService
    {
        Task<ServiceResponse<List<ApartmentGetDto>>> GetAll();
        Task<ServiceResponse<ApartmentGetDto>> GetById(int id);
    }
}
