namespace Light.Identity.EntityFrameworkCore.Services;

public class UserAttributeService(IIdentityDbContext context) : IUserAttributeService
{
    public async Task<IResult<IEnumerable<UserAttributeDto>>> GetUserAttributesAsync(string userId)
    {
        var userAttributes = await context.UserAttributes
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

        return Result<IEnumerable<UserAttributeDto>>.Success(userAttributes);
    }

    public async Task<IResult> AddAsync(string userId, string key, string value)
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

        return Result.Success();
    }

    public async Task<IResult> DeleteAsync(string userId, string key)
    {
        await context.UserAttributes
            .Where(x => x.UserId == userId && x.Key == key)
            .ExecuteDeleteAsync();

        await context.SaveChangesAsync();

        return Result.Success();
    }
}
