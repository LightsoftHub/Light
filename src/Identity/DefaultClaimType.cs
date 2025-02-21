namespace Light.Identity;

/// <summary>
/// Default claim types
/// </summary>
public class DefaultClaimType : IClaimType
{
    public virtual string UserId => "uid";

    public virtual string UserName => "un";

    public virtual string FirstName => "first_name";

    public virtual string LastName => "last_name";

    public virtual string FullName => "full_name";

    public virtual string PhoneNumber => "phone_number";

    public virtual string Email => "email";

    public virtual string Role => "role";

    public virtual string Permission => "permission";

    public virtual string ImageUrl => "image_url";

    public virtual string Expiration => "exp";

    public virtual string AccessToken => "token";
}
