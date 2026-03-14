namespace Category.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options)
        : base(options)
    {
    }

    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(256).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(32).IsRequired();
            entity.HasIndex(e => new { e.ParentId, e.Name }).IsUnique();
        });
    }
}

public sealed class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int Depth { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = "Active";
}
