namespace Light.ActiveDirectory.Dtos
{
    public class DomainUserDto
    {
        public string UserName { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string? Email { get; set; } = default!;

        public string? PhoneNumber { get; set; } = default!;
    }
}