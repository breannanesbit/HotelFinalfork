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
    }
}
