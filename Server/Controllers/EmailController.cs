using HotelFinal.Shared;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelFinal.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly MailjetClient mailClient;
        private readonly IConfiguration config;
        private readonly ILogger<EmailController> logger;

        public EmailController(IConfiguration config, ILogger<EmailController> logger)
        {
            this.mailClient = new(config["MJ_APIKEY_PUBLIC"], config["MJ_APIKEY_PRIVATE"]);
            this.config = config;
            this.logger = logger;
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
                   .WithTo(new SendContact("anthony.hardman@students.snow.edu"))
                   .Build();

            // invoke API to send email
            var response = await mailClient.SendTransactionalEmailAsync(email);
        }

        [HttpPost("reservationConfirmation")]
        public async Task SendReservationConfirmationEmail(ReservationConfirmationObject rco)
        {
            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("anthony.hardman@students.snow.edu"))
                   .WithSubject("Thank You For Making A Reservation!🎉")
                   .WithHtmlPart(
                    @$"
                        <h1>Reservation Confirmation</h1>
                        <p>
                            Your reservation has been confirmed for {rco.Checkin} to {rco.Checkout}.
                        </p>
                    ")
                   .WithTo(new SendContact(rco.UserEmail))
                   .Build();

            // invoke API to send email
            var response = await mailClient.SendTransactionalEmailAsync(email);
            logger.LogInformation($"Sent reservation confirmation to {rco.UserEmail}");
        }

        [HttpPost("reservationCancellation")]
        public async Task SendReservationCancellationEmail(ReservationConfirmationObject rco)
        {
            var email = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("anthony.hardman@students.snow.edu"))
                   .WithSubject("Your Reservation Has Been Cancelled")
                   .WithHtmlPart(
                    @$"
                        <h1>Reservation Cancellation</h1>
                        <p>
                            Your reservation for {rco.Checkin} to {rco.Checkout} has been cancelled.
                        </p>
                    ")
                   .WithTo(new SendContact(rco.UserEmail))
                   .Build();

            // invoke API to send email
            var response = await mailClient.SendTransactionalEmailAsync(email);
            logger.LogInformation($"Sent cancellation confirmation to {rco.UserEmail}");
        }
    }
}
