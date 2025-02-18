using Light.Identity.EntityFrameworkCore;

namespace Light.Identity.Services;

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
