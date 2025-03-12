using Light.Mail;
using Light.SmtpMail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sample.Models;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> _logger;

        public MailController(ILogger<MailController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //string filePath = @$"D:\\backups\\pexels-pixabay-268533.jpg";
            //byte[] byteArray = System.IO.File.ReadAllBytes(filePath);

            var from = new MailFrom("leslie.bailey@ethereal.email");

            var message = new MailMessage
            {
                Subject = "Test...." + DateTime.Now,
                Content = "Hello,.......... this test mail",
            };

            message.Recipients = ["test@yopmail.com"];

            //message.CcRecipients.Add("test1@yopmail.com");

            //message.BccRecipients.Add("test2@yopmail.com");

            //message.Attachments.Add(new MailAttachment
            //{
            //    FileName = "pexels-pixabay-268533.jpg",
            //    FileToBytes = byteArray
            //});

            var smtp = new SmtpSettings
            {
                Host = "smtp.ethereal.email",
                Port = 587,
                UseSsl = false,
                UserName = "leslie.bailey@ethereal.email",
                Password = "nrfdZH5KfTaHp6DWRF"
            };


            var smtpClient = new SmtpMailKit(smtp);
            await smtpClient.SendAsync(from, message);

            return Ok();
        }
    }
}