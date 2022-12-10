using HotelFinal.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly HotelContext hotelContext;

        public RentalController(HotelContext hotelContext)
        {
            this.hotelContext = hotelContext;
        }

        //[HttpPost]
        //public async Task CreateRental(Reservation reservation)
        //{
        //    List<RentalRoom> rentalRooms= new List<RentalRoom>();


        //    Rental rental = new Rental()
        //    {
        //        Checkin = DateOnly.FromDateTime(DateTime.Now),
        //        ReservationId = reservation.Id,
        //    };

        //    foreach (var resRoom in reservation.ReservationRooms)
        //    {
        //        var room = new RentalRoom()
        //        {
        //            RentalId = rental.Id,
        //            RoomCleaningId
        //        }
        //    }
        //}
    }
}
