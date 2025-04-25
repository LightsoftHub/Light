using Light.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Sample.Data;

public partial class AlphaDbContext : DbContext
{
    public AlphaDbContext()
    {
    }

    public AlphaDbContext(DbContextOptions<AlphaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RetailLocation> RetailLocations { get; set; }

    public virtual DbSet<RetailStore> RetailStores { get; set; }

    public virtual DbSet<RetailCategory> RetailCategories { get; set; }

    public virtual DbSet<RetailProduct> RetailProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // QueryFilters need to be applied before base.OnModelCreating
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.Deleted == null);

        modelBuilder.Entity<RetailLocation>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("Retail$Locations");

            entity.Property(e => e.Code).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<RetailStore>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("Retail$Stores");

            entity.Property(e => e.Code).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(450);
            entity.Property(e => e.LocationCode).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Note).HasMaxLength(1000);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.LocationCodeNavigation).WithMany(p => p.RetailStores)
                .HasForeignKey(d => d.LocationCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Retail$Stores_Retail$Locations");
        });

        modelBuilder.Entity<RetailCategory>(entity =>
        {
            entity.ToTable("Retail$Categories");

            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<RetailProduct>(entity =>
        {
            entity.ToTable("Retail$Products");

            entity.Property(e => e.CategoryId).HasMaxLength(450);
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.LastModifiedBy).HasMaxLength(450);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Sku).HasMaxLength(30);
            entity.Property(e => e.StoreCode).HasMaxLength(30);
            entity.Property(e => e.Unit).HasMaxLength(10);

            entity.HasOne(d => d.Category).WithMany(p => p.RetailProducts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Retail$Products_Retail$Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
