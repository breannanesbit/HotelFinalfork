using HotelFinal.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GuestController : ControllerBase
    {
        private readonly HotelContext context;
        private readonly ILogger<GuestController> logger;

        public GuestController(HotelContext context, ILogger<GuestController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("{firstname}/{lastname}")]
        public async Task<Guest> GetGuestAsync(string firstname, string lastname)
        {
            var guest = context.Guests.FirstOrDefault(g => g.FirstName == firstname && g.LastName == lastname);
            return guest;
        }

        [HttpPost]
        public async Task PostGuestAsync(Guest guest)
        {
            if (guest == null)
            {
                logger.LogInformation("Cannot Post a Null Guest");
            }
            else
            {
                context.Guests.Add(guest);
                await context.SaveChangesAsync();
            }
        }
    }
}
