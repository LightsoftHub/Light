using System.Collections.Generic;

namespace Light.Mail
{
    public class MailMessage
    {
        public class FromInfo
        {
            public FromInfo(string address)
            {
                Address = address;
            }

            public FromInfo(string address, string? displayName)
            {
                Address = address;
                DisplayName = displayName;
            }

            public string Address { get; set; } = null!;

            public string? DisplayName { get; set; }
        }

        public FromInfo From { get; set; } = null!;
        
        public List<string> Recipients { get; set; } = null!;

        public string Subject { get; set; } = default!;

        public string Content { get; set; } = default!;

        public List<string>? CcRecipients { get; set; }

        public List<string>? BccRecipients { get; set; }

        public List<MailAttachment>? Attachments { get; set; }
    }
}
