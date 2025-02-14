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
        product.CreatedOn.ShouldBe(TestValues.CreateAtTime);

        product.TenantId.ShouldBe(TestValues.TenandId);
        product.TenantId.ShouldBe(TestValues.TenandId);

        // Update
        product.Name = "Update Name";
        await SaveAsync();

        product.LastModifiedBy.ShouldBe(TestValues.UpdateByUserId);
        product.LastModifiedOn.ShouldBe(TestValues.UpdateAtTime);

        // Delete
        await RemoveAsync(product);

        product.IsDeleted.ShouldBe(true);
        product.DeletedBy.ShouldBe(TestValues.DeleteByUserId);
        product.DeletedOn.ShouldBe(TestValues.DeleteAtTime);
    }
}
