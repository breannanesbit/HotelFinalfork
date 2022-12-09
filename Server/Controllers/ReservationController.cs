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
        public ReservationController(HotelContext hotelContext)
        {
            this.hotelContext = hotelContext;
        }

        [HttpPost]
        public async Task PostReservationAsync(Reservation reservation)
        {
            await hotelContext.Reservations.AddAsync(reservation);
            await hotelContext.SaveChangesAsync();
        }

        [HttpGet("/allreservation")]
        public async Task<List<Reservation>> AllReservationAsync()
        {
            return await hotelContext.Reservations.ToListAsync();
        }
        [HttpGet("/allreservationroom")]
        public async Task<List<ReservationRoom>> AllReservationRoomsAsync()
        {
            return await hotelContext.ReservationRooms.ToListAsync();
        }

        [HttpGet("/allroomtype")]
        public async Task<List<RoomType>> AllRoomstypeAsync()
        {
            return await hotelContext.RoomTypes.ToListAsync();
        }

        [HttpGet("/guest")]
        public async Task<List<Guest>> AllGuestsAsync()
        {
            return await hotelContext.Guests.ToListAsync();
        }
    }
}
