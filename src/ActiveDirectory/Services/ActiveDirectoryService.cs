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

    public Task<bool> CheckPasswordSignInAsync(string userName, string password)
    {
        // Create a context that will allow you to connect to your Domain Controller
        using (var adContext = new PrincipalContext(ContextType.Domain, _domain.Name))
        {
            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);

            //Check user is blocked
            if (user is not null && !user.IsAccountLockedOut())
            {
                var validate = adContext.ValidateCredentials(userName, password);
                if (validate)
                {
                    return Task.FromResult(true);
                }
            }

            return Task.FromResult(false);
        }
        ;
    }

    public async Task<DomainUserDto?> GetByUserNameAsync(string userName)
    {
        using var adContext = new PrincipalContext(ContextType.Domain, _domain.Name);
        {
            var adUser = UserPrincipal.FindByIdentity(adContext, userName);

            if (adUser != null)
            {
                var result = new DomainUserDto
                {
                    UserName = adUser.UserPrincipalName,
                    FirstName = adUser.GivenName,
                    LastName = adUser.Surname,
                    PhoneNumber = adUser.VoiceTelephoneNumber,
                    Email = adUser.EmailAddress,
                };

                return await Task.FromResult(result);
            }

            return default;
        }
    }
}
