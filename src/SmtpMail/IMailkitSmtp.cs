namespace Light.SmtpMail
{
    public interface IMailkitSmtp : ISmtp
    {
        string Password { get; }

        string UserName { get; }
    }
}