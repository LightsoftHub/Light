namespace Light.Identity;

public interface IUserAttributeService
{
    Task<IEnumerable<UserAttributeDto>> GetByAsync(string userId);

    Task AddAsync(string userId, string key, string value);

    Task DeleteAsync(string userId, string key);
}
