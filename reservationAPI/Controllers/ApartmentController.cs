using Microsoft.AspNetCore.Mvc;
using reservationAPI.Models;
using reservationAPI.Services.ApartmentServices;

namespace reservationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApartmentController : Controller
    {
        private readonly IApartmentService _service;

        public ApartmentController(IApartmentService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<Apartment>>>> GetAll ()
        {
            var response = await _service.GetAll();

            if (!response.Success) 
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ServiceResponse<List<Apartment>>>> GetById(int id)
        {
            var response = await _service.GetById(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }
    }
}
