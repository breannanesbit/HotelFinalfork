using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly MailjetClient mailClient;
        private readonly IConfiguration config;

        public EmailController(IConfiguration config)
        {
            this.mailClient = new(config["MJ_APIKEY_PUBLIC"], config["MJ_APIKEY_PRIVATE"]);
            this.config = config;
        }

        [HttpPost]
        public async Task SendEmail()
        {
            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("anthony.hardman@students.snow.edu"))
                   .WithSubject("Test subject")
                   .WithHtmlPart(
                    @"
                        <h1>Header</h1>
                        <p>Your reservation has been made for November 26, 2022</p>
                    ")
                   .WithTo(new SendContact("anthonythardman@gmail.com"))
                   .Build();

            // invoke API to send email
            var response = await mailClient.SendTransactionalEmailAsync(email);
        }
    }
}
