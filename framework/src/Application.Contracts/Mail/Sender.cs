namespace Light.Mail
{
    public class Sender
    {
        public Sender(string address, string? displayName = null)
        {
            Address = address;
            DisplayName = displayName;
        }

        public string Address { get; set; }

        public string? DisplayName { get; set; }
    }
}
