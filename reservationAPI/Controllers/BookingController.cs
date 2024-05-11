using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reservationAPI.Dtos.Booking;
using reservationAPI.Models;
using reservationAPI.Services.BookingServices;
using System.Net.WebSockets;
using System.Security.Claims;

namespace reservationAPI.Controllers
{
    //Клас контролер для бронювання, доступ тільки для авторизованих користувачів
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("Booking/{id}")]
        public async Task<ActionResult<ServiceResponse<BookApartmentDto>>> Booking(int id, BookApartmentDto booking)
        {
            //За допомогою Claims знаходимо Id поточного користувача
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            //Відправляємо відповідь клієнту
            var response = await _bookingService.Booking(id, userId, booking);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("MyBookings")]
        public async Task<ActionResult<ServiceResponse<List<BookInfoDto>>>> MyBookings()
        {
            //За допомогою Claims знаходимо Id поточного користувача
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            //Відправляємо відповідь клієнту
            var response = await _bookingService.MyBookings(userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("CancelBooking/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> CancelBooking(int id)
        {
            //За допомогою Claims знаходимо Id поточного користувача
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            //Відправляємо відповідь клієнту
            var response = await _bookingService.CancelBooking(id,userId);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("EditBooking/{id}")]
        public async Task<ActionResult<ServiceResponse<BookingEditDto>>> EditBooking(int id, BookingEditDto editDto)
        {
            //За допомогою Claims знаходимо Id поточного користувача
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            //Відправляємо відповідь клієнту
            var response = await _bookingService.EditBooking(id, userId, editDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
