namespace Light.SmtpMail
{
    public class SmtpConnection : ISmtp, IMailkitSmtp
    {
        public SmtpConnection(string host, int port, bool useSsl)
        {
            Host = host;
            Port = port;
            UseSsl = useSsl;
        }

        public string Host { get; protected set; } = null!;

        public int Port { get; protected set; }

        public bool UseSsl { get; protected set; }

        public string? UserName { get; private set; }

        public string? Password { get; private set; }

        public void Authenticate(string username, string password)
        {
            UserName = username;
            Password = password;
        }
    }
}