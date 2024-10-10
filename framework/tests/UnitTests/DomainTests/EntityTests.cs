namespace UnitTests.DomainTests;

public class EntityTests
{
    [Fact]
    public void Should_Have_Id_Value_When_Default()
    {
        var entity = new DefaultEntity();

        entity.Id.Should().NotBeNullOrEmpty();

        var auditableEntity = new DefaultAuditableEntity();

        auditableEntity.Id.Should().NotBeNullOrEmpty();
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

        entity.Id.Should().Be(id);

        var auditableEntity = new CustomAuditableEntity()
        {
            Id = id
        };

        auditableEntity.Id.Should().Be(id);
    }

    [Fact]
    public void Should_Have_Events()
    {
        var domainEvent = new TestEvent();

        var entity = new DefaultEntity();
        var auditableEntity = new DefaultAuditableEntity();

        entity.AddDomainEvent(domainEvent);
        auditableEntity.AddDomainEvent(domainEvent);

        entity.DomainEvents.Should().Contain(domainEvent);
        auditableEntity.DomainEvents.Should().Contain(domainEvent);

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

        intEntity.DomainEvents.Should().Contain(domainEvent);
        intAuditableEntity.DomainEvents.Should().Contain(domainEvent);
    }
}
