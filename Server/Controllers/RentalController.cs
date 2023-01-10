using HotelFinal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Data;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly HotelContext hotelContext;
        private readonly ILogger logger;

        public RentalController(HotelContext hotelContext, ILogger logger)
        {
            this.hotelContext = hotelContext;
            this.logger = logger;
        }

        [HttpPost("checkout")]
        public async Task PostRentalCheckout(Rental rental)
        {
            rental.Checkout = DateOnly.FromDateTime(DateTime.Now);
            hotelContext.Rentals.Update(rental);
            await hotelContext.SaveChangesAsync();
        }

        [HttpGet("{reservationId}")]
        public async Task<Rental> GetRentalsForReservation(int reservationId)
        {
            return await hotelContext.Rentals
                .Where(r => r.ReservationId== reservationId)
                .FirstOrDefaultAsync();
        }

        [HttpGet("checkedIn")]
        public async Task<List<Rental>> GetCheckedInRentals()
        {
            return await hotelContext.Rentals
                .Include(r => r.Guest)
                .Where(r => r.Checkout == null).ToListAsync();
        }

        [HttpPost]
        public async Task<bool> CreateRental(RentalCreationObject rco)
        {
            rco.Reservation = await hotelContext.Reservations
                .Include(r => r.ReservationRooms)
                .Where(r => r.Id== rco.Reservation.Id).FirstOrDefaultAsync();

            Rental rental = new Rental()
            {
                Checkin = DateOnly.FromDateTime(DateTime.Now),
                ReservationId = rco.Reservation.Id,
                GuestId = rco.Reservation.GuestId,
                StaffId = rco.Staff.Id
            };

            await hotelContext.Rentals.AddAsync(rental);
            await hotelContext.SaveChangesAsync();

            List<RentalRoom> rentalRooms = new();

            foreach (var resRoom in rco.Reservation.ReservationRooms)
            {
                var roomCleaning = await hotelContext.RoomCleanings
                .Include(r => r.Room.RoomType)
                .Include(r => r.RentalRoom)
                .Where(r => r.Room.RoomTypeId == resRoom.RoomTypeId)
                .Where(r => r.RentalRoom == null)
                .Where(r => r.CleaningTypeId == 1)
                .FirstOrDefaultAsync();

                if (roomCleaning is null)
                {
                    hotelContext.Rentals.Remove(rental);
                    hotelContext.SaveChanges();
                    return false;
                }

                var room = new RentalRoom()
                {
                    RentalId = rental.Id.Value,
                    RoomCleaningId = roomCleaning.Id.Value,
                    RentalRate = 150.00m
                };

               rentalRooms.Add(room);
                logger.LogInformation("rental for {room} was complete", room);
            }

            
            foreach (var room in rentalRooms)
            {
                room.Id = rental.Id.Value;
                await hotelContext.RentalRooms.AddAsync(room);
                await hotelContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
