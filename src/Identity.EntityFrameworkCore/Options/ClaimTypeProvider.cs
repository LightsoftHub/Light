namespace Light.Identity.Options;

internal class ClaimTypeProvider : IClaimType
{
    public string UserId => DefaultClaimType.UserId;

    public string UserName => DefaultClaimType.UserName;

    public string FirstName => DefaultClaimType.FirstName;

    public string LastName => DefaultClaimType.LastName;

    public string FullName => DefaultClaimType.LastName;

    public string PhoneNumber => DefaultClaimType.PhoneNumber;

    public string Email => DefaultClaimType.Email;

    public string Role => DefaultClaimType.Role;

    public string Permission => DefaultClaimType.Permission;

    public string ImageUrl => DefaultClaimType.ImageUrl;

    public string Expiration => DefaultClaimType.Expiration;

    public string AccessToken => DefaultClaimType.AccessToken;
}
