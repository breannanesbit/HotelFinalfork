using HotelFinal.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly HotelContext hotelContext;
        private readonly ILogger<ReservationController> ilogger;

        public ReservationController(HotelContext hotelContext, ILogger<ReservationController> ilogger)
        {
            this.hotelContext = hotelContext;
            this.ilogger = ilogger;
        }

        [HttpGet("allreservation")]
        public async Task<List<Reservation>> AllReservationAsync()
        {
            return await hotelContext.Reservations.ToListAsync();
        }
        [HttpGet("allreservationroom")]
        public async Task<List<ReservationRoom>> AllReservationRoomsAsync()
        {
            var list =  await hotelContext.ReservationRooms.Include(r => r.Reservation).Include(r => r.RoomType).ToListAsync();
            ilogger.LogDebug("made it to the reservationroom");
            return list;
        }

        [HttpGet("allroomtype")]
        public async Task<List<RoomType>> AllRoomstypeAsync()
        {
            return await hotelContext.RoomTypes.ToListAsync();
        }

        [HttpGet("guest")]
        public async Task<List<Guest>> AllGuestsAsync()
        {
            return await hotelContext.Guests.ToListAsync();
        }
    }
}
