using Light.Domain.Entities;

namespace UnitTests.DomainTests;

internal class CustomEntity : BaseEntity<int>
{ }

internal class CustomAuditableEntity : BaseAuditableEntity<int>
{ }

