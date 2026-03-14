namespace Product.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ProductReadDbContext : DbContext
{
    public ProductReadDbContext(DbContextOptions<ProductReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Variants).WithOne().HasForeignKey(v => v.ProductId);
            entity.HasMany(e => e.Media).WithOne().HasForeignKey(m => m.ProductId);
        });

        modelBuilder.Entity<ProductVariantEntity>(entity =>
        {
            entity.ToTable("ProductVariants");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ProductMediaEntity>(entity =>
        {
            entity.ToTable("ProductMedia");
            entity.HasKey(e => e.Id);
        });
    }
}
