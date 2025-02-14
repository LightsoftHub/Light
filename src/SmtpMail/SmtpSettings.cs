namespace Light.SmtpMail
{
    public class SmtpSettings
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }
    }
}