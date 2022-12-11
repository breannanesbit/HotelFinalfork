using HotelFinal.Client.Pages;
using HotelFinal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly HotelContext hotelContext;
        private readonly ILogger<RoomController> ilogger;

        public RoomController(HotelContext hotelContext, ILogger<RoomController> ilogger)
        {
            this.hotelContext = hotelContext;
            this.ilogger = ilogger;
        }

        [HttpGet("valid/{roomNumber}")]
        public async Task<bool> IsValidRoom(int roomNumber)
        {
            var room = await hotelContext.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);

            if (room == null)
            {
                return false;
                ilogger.LogError("room number was not an valid room number");
            }

            return true;
            ilogger.LogDebug("found room number");
        }

        [HttpGet("{roomNumber}")]
        public async Task<Room> GetRoomFromRoomNumber(int roomNumber)
        {
            return await hotelContext.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }

        [AllowAnonymous]
        [HttpGet("roomtype")]
        public async Task<List<RoomType>> GetRoomTypesAsync()
        {
            return await hotelContext.RoomTypes.ToListAsync();
        }

        [HttpGet("cleanrooms")]
        public async Task<List<RoomCleaningInfo>> AllCleanRooms()
        {
            return await hotelContext.RoomCleaningInfos.ToListAsync();
        }

        [HttpGet("allreservationroom")]
        public async Task<List<ReservationRoom>> AllReservationRoomsAsync()
        {
            var list = await hotelContext.ReservationRooms.Include(r => r.Reservation).ThenInclude(r => r.Guest).Include(r => r.RoomType).ToListAsync();
            ilogger.LogDebug("made it to the reservationroom");
            return list;
        }

        [HttpGet("availableRoom/{start}/{end}")]
        public async Task<List<RentalRoom>> GetAvailableRooms(DateTime start, DateTime end)
        {
            return await hotelContext.RentalRooms.Include(r => r.Rental)
                .Where(r => r.Rental.Checkin >= DateOnly.FromDateTime(start) && (r.Rental.Checkout == null || r.Rental.Checkout <= DateOnly.FromDateTime(end))).Include(r => r.RoomCleaning)
                .ThenInclude(r => r.Room).ToListAsync();
        }

        [HttpGet("roomTypeCounts")]
        public async Task<Dictionary<int, int>> GetCountOfRooms()
        {
            Dictionary<int, int> roomTypeCounts = new();
            var rooms = hotelContext.Rooms.ToList();

            foreach(var room in rooms)
            {
                if (roomTypeCounts.ContainsKey(room.RoomTypeId))
                {
                    roomTypeCounts[room.RoomTypeId]++;
                }
                else
                {
                    roomTypeCounts[room.RoomTypeId] = 1;
                }
            }

            return roomTypeCounts;
        }

        [HttpGet("availableRoomTypes/{start}/{end}")]
        public async Task<List<RoomType>> GetAvailableRoomTypes(DateTime start, DateTime end)
        {
            Dictionary<int, int> roomCounts = await GetCountOfRooms();
            var reservations = await hotelContext.Reservations
                .Where(r => r.ExpectedCheckin >= DateOnly.FromDateTime(start) && r.ExpectedCheckout <= DateOnly.FromDateTime(end))
                .ToListAsync();
            var reservationRooms = await hotelContext.ReservationRooms.ToListAsync();
            var roomTypes = await hotelContext.RoomTypes.ToListAsync();

            var availableRoomCounts = GetAvalibleRoomCounts(roomCounts, reservations, reservationRooms);

            List<RoomType> availableRoomsInDateRange = FilterEmptyRoomTypes(availableRoomCounts, roomTypes);

            ilogger.LogDebug("made it to get all available rooms");
            return availableRoomsInDateRange;
        }

        [NonAction]
        public Dictionary<int, int> GetAvalibleRoomCounts(Dictionary<int, int> roomTypeCounts, List<Reservation> reservations, List<ReservationRoom> reservationRooms)
        {
            foreach (var res in reservations)
            {
                var resRooms = reservationRooms.Where(r => r.ReservationId == res.Id).ToList();

                foreach (var room in resRooms)
                {
                    if (roomTypeCounts.ContainsKey(room.RoomTypeId))
                    {
                        roomTypeCounts[room.RoomTypeId]--;
                        ilogger.LogInformation("subtracting if we find a room that is occupided");
                    }
                }
            }
            return roomTypeCounts;
        }

        [NonAction]
        public List<RoomType> FilterEmptyRoomTypes(Dictionary<int, int> roomTypeCounts, List<RoomType> roomTypes)
        {
            List<RoomType> availableRoomsInDateRange = new();

            foreach (var roomType in roomTypeCounts)
            {
                if (roomType.Value > 0)
                {
                    availableRoomsInDateRange.Add(roomTypes.FirstOrDefault(r => r.Id == roomType.Key));
                }
            }

            return availableRoomsInDateRange;
        }

    }
}
