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

        public RoomController() { }

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

            var availableRoomCounts = GetNumberOfAvalibleRooms(roomCounts, reservations, reservationRooms);

            List<RoomType> availableRoomsInDateRange = FilterEmptyRoomTypes(availableRoomCounts, roomTypes);

            return availableRoomsInDateRange;
        }

        public Dictionary<int, int> GetNumberOfAvalibleRooms(Dictionary<int, int> roomTypeCounts, List<Reservation> reservations, List<ReservationRoom> reservationRooms)
        {
            foreach (var res in reservations)
            {
                var resRooms = reservationRooms.Where(r => r.ReservationId == res.Id).ToList();

                foreach (var room in resRooms)
                {
                    if (roomTypeCounts.ContainsKey(room.RoomTypeId))
                    {
                        roomTypeCounts[room.RoomTypeId]--;
                    }
                }
            }
            return roomTypeCounts;
        }

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
