using Light.Identity.Options;
using Microsoft.Extensions.Options;

namespace WebApi.Data;

public class CustomClaims : IConfigureOptions<ClaimTypeOptions>
{
    public void Configure(ClaimTypeOptions options)
    {
        options.Permission = Permission;
        options.UserId = UserId;
        options.UserName = UserName;
        options.FirstName = FirstName;
        options.LastName = LastName;
        options.FullName = FullName;
    }

    public const string Permission = "custom_permission";
    public const string UserId = "custom_uid";
    public const string UserName = "custom_un";
    public const string FirstName = "custom_first_name";
    public const string LastName = "custom_last_name";
    public const string FullName = "custom_full_name";
    public const string PhoneNumber = "phone_number";
    public const string Email = "email";
    public const string Role = "role";
    public const string ImageUrl = nameof(ImageUrl);
    public const string Expiration = "exp";
    public const string AccessToken = "token";
}
