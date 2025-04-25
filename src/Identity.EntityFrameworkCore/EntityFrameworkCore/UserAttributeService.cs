namespace Light.Identity.EntityFrameworkCore;

public class UserAttributeService(IIdentityContext context) : IUserAttributeService
{
    public async Task<IEnumerable<UserAttributeDto>> GetByAsync(string userId)
    {
        return await context.UserAttributes
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .Select(s => new UserAttributeDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Key = s.Key,
                Value = s.Value
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(string key, string value)
    {
        var activeUserQuery = context.Users
            .Where(x =>
                x.Status.Value == IdentityStatus.active
                && x.Deleted == null);

        var userAttributeQuery = context.UserAttributes
            .Where(x => x.Key == key && x.Value == value);

        return await userAttributeQuery
            .Join(activeUserQuery,
                userAttribute => userAttribute.UserId,
                user => user.Id,
                (userAttribute, user) => user)
            .AsNoTracking()
            .Select(s => s.MapToDto())
            .ToListAsync();
    }

    public async Task AddAsync(string userId, string key, string value)
    {
        var userAttribute = await context.UserAttributes
            .Where(x => x.UserId == userId && x.Key == key)
            .SingleOrDefaultAsync();

        if (userAttribute is not null)
        {
            userAttribute.Value = value;
        }
        else
        {
            var entity = new UserAttribute
            {
                UserId = userId,
                Key = key,
                Value = value
            };

            await context.UserAttributes.AddAsync(entity);
        }

        await context.SaveChangesAsync();
    }

    public Task DeleteAsync(string userId, string key)
    {
        return context.UserAttributes
            .Where(x => x.UserId == userId && x.Key == key)
            .ExecuteDeleteAsync();
    }
}
