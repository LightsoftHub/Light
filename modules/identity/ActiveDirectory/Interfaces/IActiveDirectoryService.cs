using Light.ActiveDirectory.Dtos;

namespace Light.ActiveDirectory.Interfaces
{
    public interface IActiveDirectoryService
    {
        /// <summary>
        /// Check userName & password from Active Directory
        /// </summary>
        Task<IResult> CheckPasswordSignInAsync(string userName, string password);

        /// <summary>
        /// Get User Infomation from Active Directory
        /// </summary>
        Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName);
    }
}