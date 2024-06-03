using Light.ActiveDirectory.Dtos;
using Light.ActiveDirectory.Interfaces;

namespace Light.ActiveDirectory.Services;

public class FakeActiveDirectoryService : IActiveDirectoryService
{
    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        IResult result = Result.Unauthorized("Invalid credentials.");

        return Task.FromResult(result);
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        IResult<DomainUserDto> result = Result<DomainUserDto>.NotFound("DomainUser", userName);

        return Task.FromResult(result);
    }
}
