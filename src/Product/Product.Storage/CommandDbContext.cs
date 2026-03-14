namespace Product.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<ProductVariantEntity> ProductVariants => Set<ProductVariantEntity>();
    public DbSet<ProductMediaEntity> ProductMedia => Set<ProductMediaEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Status).HasMaxLength(32).IsRequired();
            entity.HasMany(e => e.Variants).WithOne().HasForeignKey(v => v.ProductId);
            entity.HasMany(e => e.Media).WithOne().HasForeignKey(m => m.ProductId);
        });

        modelBuilder.Entity<ProductVariantEntity>(entity =>
        {
            entity.ToTable("ProductVariants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Size).HasMaxLength(32);
            entity.Property(e => e.Color).HasMaxLength(32);
            entity.Property(e => e.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<ProductMediaEntity>(entity =>
        {
            entity.ToTable("ProductMedia");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Url).HasMaxLength(1024).IsRequired();
            entity.Property(e => e.Alt).HasMaxLength(256);
        });
    }
}

public sealed class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string Status { get; set; } = "Draft";
    public List<ProductVariantEntity> Variants { get; set; } = new();
    public List<ProductMediaEntity> Media { get; set; } = new();
}

public sealed class ProductVariantEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public sealed class ProductMediaEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Alt { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
