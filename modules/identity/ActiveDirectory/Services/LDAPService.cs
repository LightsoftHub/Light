using Light.ActiveDirectory.Dtos;
using Light.ActiveDirectory.Interfaces;
using Light.ActiveDirectory.Options;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace Light.ActiveDirectory.Services;

[SupportedOSPlatform("windows")]
public class LDAPService(IOptions<LdapOptions> options) : IActiveDirectoryService
{
    private readonly LdapOptions _options = options.Value;

    public bool IsConfigured() => true;

    [SupportedOSPlatform("windows")]
    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        IResult result = Result.Unauthorized("Invalid credentials.");

        try
        {
            if (string.IsNullOrEmpty(password.Trim()))
            {
                return Task.FromResult(result);
            }
            // create LDAP connection
            var ldapConn = new LdapConnection() { SecureSocketLayer = false };

            // create socket connect to server
            ldapConn.Connect(_options.Address, _options.Port);

            // bind domain user with domain name (username@domain.com) & password
            ldapConn.Bind(userName + "@" + _options.Name, password);

            result = Result.Success();
        }
        catch (Exception ex)
        {
            result = Result.Error(ex.Message);
        }

        return Task.FromResult(result);
    }

    public IResult ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        var sPath = _options.Connection; // This is if your domain was my.domain.com
        var de = new DirectoryEntry(sPath, _options.UserName, _options.Password, AuthenticationTypes.Secure);
        var ds = new DirectorySearcher(de);
        string qry = string.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))", userName);
        ds.Filter = qry;
        try
        {
            var sr = ds.FindOne();
            if (sr is null)
            {
                return Result.Error("User not found on domain.");
            }

            DirectoryEntry user = sr.GetDirectoryEntry();
            user.Invoke("SetPassword", new object[] { newPassword });
            user.CommitChanges();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}