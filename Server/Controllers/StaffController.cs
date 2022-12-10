using HotelFinal.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly HotelContext context;
        private readonly ILogger<StaffController> logger;

        public StaffController(HotelContext context, ILogger<StaffController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("{firstname}/{lastname}")]
        public async Task<Staff> GetStaffAsync(string firstname, string lastname)
        {
            var staff = context.Staff.FirstOrDefault(s => s.FirstName == firstname && s.LastName == lastname);
            return staff;
        }

        [HttpPost]
        public async Task PostGuestAsync(Staff staff)
        {
            if (staff == null)
            {
                logger.LogWarning("Cannot Post a Null Staff Member");
            }
            else
            {
                context.Staff.Add(staff);
                await context.SaveChangesAsync();
            }
        }
    }
}
