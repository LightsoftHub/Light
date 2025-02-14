namespace UnitTests.DomainTests;

public class EntityTests
{
    [Fact]
    public void Should_Have_Id_Value_When_Default()
    {
        var entity = new DefaultEntity();

        entity.Id.ShouldNotBeNullOrEmpty();

        var auditableEntity = new DefaultAuditableEntity();

        auditableEntity.Id.ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Should_Have_Return_Correct_Id(int id)
    {
        var entity = new CustomEntity()
        {
            Id = id
        };

        entity.Id.ShouldBe(id);

        var auditableEntity = new CustomAuditableEntity()
        {
            Id = id
        };

        auditableEntity.Id.ShouldBe(id);
    }

    [Fact]
    public void Should_Have_Events()
    {
        var domainEvent = new TestEvent();

        var entity = new DefaultEntity();
        var auditableEntity = new DefaultAuditableEntity();

        entity.AddDomainEvent(domainEvent);
        auditableEntity.AddDomainEvent(domainEvent);

        entity.DomainEvents.ShouldContains(domainEvent);
        auditableEntity.DomainEvents.ShouldContains(domainEvent);

        var intEntity = new CustomEntity()
        {
            Id = 1
        };

        var intAuditableEntity = new CustomAuditableEntity()
        {
            Id = 1
        };

        intEntity.AddDomainEvent(domainEvent);
        intAuditableEntity.AddDomainEvent(domainEvent);

        intEntity.DomainEvents.ShouldContains(domainEvent);
        intAuditableEntity.DomainEvents.ShouldContains(domainEvent);
    }
}
