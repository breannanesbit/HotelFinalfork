using HotelFinal.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly HotelContext hotelContext;
        public ReservationController(HotelContext hotelContext)
        {
            this.hotelContext = hotelContext;
        }

        [HttpGet("/all")]
        public async Task<List<Reservation>> AllReservationAsync()
        {
            return await hotelContext.Reservations.ToListAsync();
        }

        [HttpGet("/guest")]
        public async Task<List<Guest>> AllGuestsAsync()
        {
            return await hotelContext.Guests.ToListAsync();
        }
    }
}
