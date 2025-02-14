namespace Light.Identity;

public interface IClaimType
{
    string AccessToken { get; }
    string Email { get; }
    string Expiration { get; }
    string FirstName { get; }
    string FullName { get; }
    string ImageUrl { get; }
    string LastName { get; }
    string Permission { get; }
    string PhoneNumber { get; }
    string Role { get; }
    string UserId { get; }
    string UserName { get; }
}
