using HotelFinal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class ReservationController : ControllerBase
    {
        private readonly HotelContext hotelContext;
        private readonly ILogger<ReservationController> ilogger;

        public ReservationController(HotelContext hotelContext, ILogger<ReservationController> ilogger)
        {
            this.hotelContext = hotelContext;
            this.ilogger = ilogger;
        }

        [HttpPost]
        public async Task PostReservationAsync(ReservationPostObject rpo)
        {
            await hotelContext.Reservations.AddAsync(rpo.Reservation);
            await hotelContext.SaveChangesAsync();

            foreach (var type in rpo.RoomTypes)
            {
                var room = new ReservationRoom()
                {
                    ReservationId = rpo.Reservation.Id.Value,
                    RoomTypeId = type.Id,
                };
                await hotelContext.ReservationRooms.AddAsync(room);
                await hotelContext.SaveChangesAsync();
            }
        }

        [HttpGet("allreservation")]
        public async Task<List<Reservation>> AllReservationAsync()
        {
            return await hotelContext.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Rentals)
                .Include(r => r.ReservationRooms)
                .ToListAsync();
        }

        [HttpPost("cancel")]
        public async Task CancelReservation(Reservation reservation)
        {
            hotelContext.Reservations.Remove(reservation);
            await hotelContext.SaveChangesAsync();
        }

        [HttpGet("unfulfilled")]
        public async Task<List<Reservation>> GetUnfullfilledReservations()
        {
            return await hotelContext.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Rentals)
                .Where(r => r.Rentals.Count  == 0)
                .ToListAsync();
        }

        [HttpGet("noRentalAllReservations")]
        public async Task<List<Reservation>> AllReservationsWithouRentals()
        {
            return await hotelContext.Reservations
                .Include(r => r.Guest)
                .Include(r => r.Rentals)
                .Include(r => r.ReservationRooms)
                .Where(r => r.Rentals.Count() == 0)
                .ToListAsync();
        }

        [HttpGet("allreservationroom")]
        public async Task<List<ReservationRoom>> AllReservationRoomsAsync()
        {
            var list =  await hotelContext.ReservationRooms.Include(r => r.Reservation)
                .Include(r => r.RoomType)
                .ToListAsync();

            var userName = User.Identity?.IsAuthenticated ?? false ? User.Identity.Name : "";
            using (ilogger.BeginScope("ScopeUser: {scopeUser}, ScopedCat: {scopedCat}", userName, "reservation")) ;
            {
                //ilogger.LogInformation("made it to the reservationroom to {userName}", userName);
                LogResveration();
            }
            return list;
        }

        [LoggerMessage(1, LogLevel.Information, "made it to the reservation room")]
        partial void LogResveration();

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
