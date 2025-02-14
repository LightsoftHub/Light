using Microsoft.EntityFrameworkCore;

namespace IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static TestDbContext _context;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // Set up the InMemory database options
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new TestDbContext(options);
    }

    public static DbSet<TEntity> Set<TEntity>()
        where TEntity : class
    {
        return _context.Set<TEntity>();
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await _context.Set<TEntity>().AddAsync(entity);

        await _context.SaveChangesAsync();
    }

    public static async Task RemoveAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        _context.Set<TEntity>().Remove(entity);

        await _context.SaveChangesAsync();
    }

    public static async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    [OneTimeTearDown]
    public async Task RunAfterAnyTests()
    {
        // Clean up database after each test
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}
