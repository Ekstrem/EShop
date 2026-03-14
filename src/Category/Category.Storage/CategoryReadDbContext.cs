namespace Category.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CategoryReadDbContext : DbContext
{
    public CategoryReadDbContext(DbContextOptions<CategoryReadDbContext> options)
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
        });
    }
}
