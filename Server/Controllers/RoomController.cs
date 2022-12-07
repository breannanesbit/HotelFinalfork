using HotelFinal.Client.Pages;
using HotelFinal.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RoomController : ControllerBase
    {
        private readonly HotelContext hotelContext;

        public RoomController(HotelContext hotelContext)
        {
            this.hotelContext = hotelContext;
        }
    
        [HttpGet("roomtype")]
        public async Task<List<RoomType>> GetRoomTypesAsync()
        {
            return await hotelContext.RoomTypes.ToListAsync();
        }

        [HttpGet("roomTypeCounts")]
        public async Task<Dictionary<int, int>> GetCountOfRoomTypesAsync()
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
            Dictionary<int, int> roomTypeCounts = await GetCountOfRoomTypesAsync();
            //Dictionary<RoomType, int> availableRooms = await GetCountOfRoomTypesAsync();
            var reservations = await hotelContext.Reservations
                .Where(r => r.ExpectedCheckin >= DateOnly.FromDateTime(start) && r.ExpectedCheckout <= DateOnly.FromDateTime(end))
                .ToListAsync();

            roomTypeCounts = GetNumberOfAvalibleRooms(roomTypeCounts, reservations);

            List<RoomType> availableRoomsInDateRange = GetRoomTypesInDateRange(roomTypeCounts);

            return availableRoomsInDateRange;
        }

        private List<RoomType> GetRoomTypesInDateRange(Dictionary<int, int> roomTypeCounts)
        {
            List<RoomType> availableRoomsInDateRange = new();

            foreach (var roomType in roomTypeCounts)
            {
                if (roomType.Value > 0)
                {
                    availableRoomsInDateRange.Add(hotelContext.RoomTypes.FirstOrDefault(r => r.Id == roomType.Key));
                }
            }

            return availableRoomsInDateRange;
        }

        private Dictionary<int, int> GetNumberOfAvalibleRooms(Dictionary<int, int> roomTypeCounts, List<Reservation> reservations)
        {
            foreach (var res in reservations)
            {
                var reservationRooms = hotelContext.ReservationRooms.Where(r => r.ReservationId == res.Id).ToList();

                foreach (var room in reservationRooms)
                {
                    if (roomTypeCounts.ContainsKey(room.RoomTypeId))
                    {
                        roomTypeCounts[room.RoomTypeId]--;
                    }
                }
            }
            return roomTypeCounts;
        }
    }
}
