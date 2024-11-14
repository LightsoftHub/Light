using Light.ActiveDirectory.Dtos;
using Light.ActiveDirectory.Interfaces;
using Light.ActiveDirectory.Options;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;

namespace Light.ActiveDirectory.Services;

[SupportedOSPlatform("windows")]
public class ActiveDirectoryService(IOptions<DomainOptions> domain) : IActiveDirectoryService
{
    private readonly DomainOptions _domain = domain.Value;

    public bool IsConfigured() => !string.IsNullOrEmpty(_domain.Name);

    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        // Create a context that will allow you to connect to your Domain Controller
        using (var adContext = new PrincipalContext(ContextType.Domain, _domain.Name))
        {
            IResult result = Result.Unauthorized("Invalid credentials.");

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);
            if (user is not null && !user.IsAccountLockedOut())
            {
                //Check user is blocked
                var validate = adContext.ValidateCredentials(userName, password);
                if (validate)
                {
                    result = Result.Success();
                }
            }

            return Task.FromResult(result);
        };
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        using var adContext = new PrincipalContext(ContextType.Domain, _domain.Name);
        {
            var adUser = UserPrincipal.FindByIdentity(adContext, userName);

            IResult<DomainUserDto> result = Result<DomainUserDto>.NotFound("DomainUser", userName);

            if (adUser != null)
            {
                result = Result<DomainUserDto>.Success(new DomainUserDto
                {
                    UserName = adUser.UserPrincipalName,
                    FirstName = adUser.GivenName,
                    LastName = adUser.Surname,
                    PhoneNumber = adUser.VoiceTelephoneNumber,
                    Email = adUser.EmailAddress,
                });
            }

            return Task.FromResult(result);
        }
    }
}
