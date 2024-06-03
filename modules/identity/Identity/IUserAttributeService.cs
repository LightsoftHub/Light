namespace Light.Identity;

public interface IUserAttributeService
{
    Task<IResult<IEnumerable<UserAttributeDto>>> GetUserAttributesAsync(string userId);

    Task<IResult> AddAsync(string userId, string key, string value);

    Task<IResult> DeleteAsync(string userId, string key);
}
