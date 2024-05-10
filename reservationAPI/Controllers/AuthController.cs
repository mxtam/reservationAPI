using Microsoft.AspNetCore.Mvc;
using reservationAPI.Data;
using reservationAPI.Dtos.User;
using reservationAPI.Models;

namespace reservationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository= authRepository;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request) 
        {
            var response = await _authRepository.Register(new User { Email = request.Email, 
                FirstName = request.FirstName, LastName = request.LastName }, request.Password);

            if (!response.Success) 
            { 
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Email, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


    }
}
