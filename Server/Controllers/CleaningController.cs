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
    public class CleaningController : ControllerBase
    {
        private readonly HotelContext hotelContext;

        public CleaningController(HotelContext hotelContext)
        {
            this.hotelContext = hotelContext;
        }

        [HttpPost]
        public async Task RecordRoomCleaning(RoomCleaning roomCleaning)
        {
            await hotelContext.RoomCleanings.AddAsync(roomCleaning);
            await hotelContext.SaveChangesAsync();
        }

        [HttpGet("types")]
        public async Task<List<CleaningType>> GetCleaningTypesAsync()
        {
            return await hotelContext.CleaningTypes.ToListAsync();
        }

        [HttpGet("occupancy/{roomTypeId}")]
        public async Task<RoomCleaning> GetFreeRoomCleaning(int roomTypeId)
        {
            var roomCleaning = await hotelContext.RoomCleanings
                .Include(r => r.RentalRoom)
                .Where(r => r.RentalRoom == null)
                .FirstOrDefaultAsync();

            return roomCleaning;
        }

        [HttpGet("info")]
        public async Task<List<RoomCleaning>> GetRoomCleaningInfo()
        {
            var cleanings = await hotelContext.RoomCleanings
                .Include(r => r.Room)
                .ThenInclude(r => r.RoomType)
                .Include(r => r.Staff)
                .Include(r => r.CleaningType)
                .Include(r => r.RentalRoom)
                .ToListAsync();

            return cleanings;
        }

    }
}
