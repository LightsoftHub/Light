using Light.Mail;
using Light.SmtpMail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly SmtpConnection _settings;
        private readonly ILogger<MailController> _logger;

        public MailController(IOptions<SmtpConnection> settings,
            ILogger<MailController> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string filePath = @$"D:\\backups\\pexels-pixabay-268533.jpg";
            byte[] byteArray = System.IO.File.ReadAllBytes(filePath);

            var from = new MailFrom("zord.contactus@gmail.com", "ZORD - Contact Us");

            var message = new MailMessage
            {
                Subject = "Test...." + DateTime.Now,
                Content = "Hello,.......... this test mail",
            };

            message.Recipients.Add("test@yopmail.com");

            //message.CcRecipients.Add("test1@yopmail.com");

            //message.BccRecipients.Add("test2@yopmail.com");

            //message.Attachments.Add(new MailAttachment
            //{
            //    FileName = "pexels-pixabay-268533.jpg",
            //    FileToBytes = byteArray
            //});

            var smtp = new SmtpMailKit();
            await smtp.SendAsync(from, message, _settings);

            return Ok();
        }
    }
}