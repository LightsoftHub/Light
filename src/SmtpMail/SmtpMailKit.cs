using Light.Mail;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading;
using System.Threading.Tasks;

namespace Light.SmtpMail
{
    public class SmtpMailKit
    {
        private readonly IMailkitSmtp _smtp;

        public SmtpMailKit(IMailkitSmtp smtp)
        {
            _smtp = smtp;
        }

        public async Task SendAsync(MailFrom from, MailMessage mail, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(from.DisplayName, from.Address),
                Subject = mail.Subject,
            };

            var bodyBuilder = new BodyBuilder { HtmlBody = mail.Content };

            // add address mail to send
            foreach (var address in mail.Recipients)
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            if (mail.CcRecipients != null)
            {
                // add CC
                foreach (var address in mail.CcRecipients)
                {
                    email.Cc.Add(MailboxAddress.Parse(address));
                }
            }

            if (mail.BccRecipients != null)
            {
                // add BCC
                foreach (var address in mail.BccRecipients)
                {
                    email.Bcc.Add(MailboxAddress.Parse(address));
                }
            }

            if (mail.Attachments != null)
            {
                foreach (var attachment in mail.Attachments)
                {
                    // file from stream
                    bodyBuilder.Attachments.Add(attachment.FileName, attachment.FileToBytes);
                }
            }

            // build message body
            email.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_smtp.Host, _smtp.Port, _smtp.UseSsl, cancellationToken);
            await smtpClient.AuthenticateAsync(_smtp.UserName, _smtp.Password, cancellationToken);
            await smtpClient.SendAsync(email, cancellationToken);
            await smtpClient.DisconnectAsync(true, cancellationToken);
            smtpClient.Dispose();
        }
    }
}