using FluentAssertions;

namespace IntegrationTests;

using static Testing;

public class DomainEntityTests : BaseTestFixture
{
    [Test]
    public async Task MustSaveAuditData()
    {
        // Arrange
        var product = new Product { Name = "Product Name" };

        // Create
        await AddAsync(product);

        product.CreatedBy.Should().Be(TestValues.CreateByUserId);
        product.CreatedOn.Should().Be(TestValues.CreateAtTime);

        product.TenantId.Should().Be(TestValues.TenandId);
        product.TenantId.Should().Be(TestValues.TenandId);

        // Update
        product.Name = "Update Name";
        await SaveAsync();

        product.LastModifiedBy.Should().Be(TestValues.UpdateByUserId);
        product.LastModifiedOn.Should().Be(TestValues.UpdateAtTime);

        // Delete
        await RemoveAsync(product);

        product.IsDeleted.Should().Be(true);
        product.DeletedBy.Should().Be(TestValues.DeleteByUserId);
        product.DeletedOn.Should().Be(TestValues.DeleteAtTime);
    }
}
