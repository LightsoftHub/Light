namespace Light.SmtpMail
{
    public interface ISmtp
    {
        string Host { get; }

        int Port { get; }

        bool UseSsl { get; }
    }
}