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
    public async Task<bool> CheckPasswordSignInAsync(string userName, string password)
    {
        if (string.IsNullOrEmpty(password.Trim()))
        {
            return false;
        }
        // create LDAP connection
        var ldapConn = new LdapConnection() { SecureSocketLayer = false };

        // create socket connect to server
        await ldapConn.ConnectAsync(_options.Address, _options.Port);

        // bind domain user with domain name (username@domain.com) & password
        await ldapConn.BindAsync(userName + "@" + _options.Name, password);

        return true;
    }

    public bool ChangePasswordAsync(string userName, string newPassword)
    {
        var sPath = _options.Connection; // This is if your domain was my.domain.com
        var de = new DirectoryEntry(sPath, _options.UserName, _options.Password, AuthenticationTypes.Secure);
        var ds = new DirectorySearcher(de);
        string qry = string.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))", userName);
        ds.Filter = qry;
        var sr = ds.FindOne();
        if (sr is null)
        {
            return false;
        }

        DirectoryEntry user = sr.GetDirectoryEntry();
        user.Invoke("SetPassword", [newPassword]);
        user.CommitChanges();

        return true;
    }

    public Task<DomainUserDto?> GetByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}