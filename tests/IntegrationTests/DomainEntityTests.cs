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

        product.CreatedBy.ShouldBe(TestValues.CreateByUserId);
        product.Created.ShouldBe(TestValues.CreateAtTime);

        product.TenantId.ShouldBe(TestValues.TenandId);
        product.TenantId.ShouldBe(TestValues.TenandId);

        // Update
        product.Name = "Update Name";
        await SaveAsync();

        product.LastModifiedBy.ShouldBe(TestValues.UpdateByUserId);
        product.LastModified.ShouldBe(TestValues.UpdateAtTime);

        // Delete
        await RemoveAsync(product);

        product.DeletedBy.ShouldBe(TestValues.DeleteByUserId);
        product.Deleted.ShouldBe(TestValues.DeleteAtTime);
    }
}
