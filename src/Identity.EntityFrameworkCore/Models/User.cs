using Light.Domain.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Light.Identity.Models;

public class User : IdentityUser, IEntity, IAuditableEntity, ISoftDelete
{
    public User() => Id = LightId.NewId();

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public bool UseDomainPassword { get; set; }

    public Status Status { get; set; } = new();

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? Deleted { get; set; }

    public string? DeletedBy { get; set; }

    public void UpdateInfo(string? firstName, string? lastName, string? phoneNumber, string? email)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
    }

    public void UpdateStatus(IdentityStatus status)
    {
        // only update 2 status
        if (status == IdentityStatus.active || status == IdentityStatus.locked)
            Status.Update(status);
    }

    public void ConnectDomain(bool connect)
    {
        // auth user via Active Directory instead local password
        UseDomainPassword = connect;
    }

    public void Delete()
    {
        UserName = null;
        FirstName = null;
        LastName = null;
        PhoneNumber = null;
        Email = null;
        PasswordHash = null;
        UseDomainPassword = false;
        Status.Update(IdentityStatus.unactive);
    }
}
