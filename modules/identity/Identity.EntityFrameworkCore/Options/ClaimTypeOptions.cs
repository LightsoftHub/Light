namespace Light.Identity.Options;

public class ClaimTypeOptions
{
    public string UserId { get; set; } = ClaimTypes.UserId;

    public string UserName { get; set; } = ClaimTypes.UserName;

    public string FirstName { get; set; } = ClaimTypes.FirstName;

    public string LastName { get; set; } = ClaimTypes.LastName;

    public string FullName { get; set; } = ClaimTypes.LastName;

    public string PhoneNumber { get; set; } = ClaimTypes.PhoneNumber;

    public string Email { get; set; } = ClaimTypes.Email;

    public string Role { get; set; } = ClaimTypes.Role;

    public string Permission { get; set; } = ClaimTypes.Permission;

    public string ImageUrl { get; set; } = ClaimTypes.ImageUrl;

    public string Expiration { get; set; } = ClaimTypes.Expiration;

    public string AccessToken { get; set; } = ClaimTypes.AccessToken;

    public string TenantId { get; set; } = ClaimTypes.TenantId;
}
