namespace Promotion.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<PromotionEntity> Promotions => Set<PromotionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PromotionEntity>(entity =>
        {
            entity.ToTable("promotions");
            entity.HasKey(e => e.Id);
            entity.HasNoKey();
        });
    }
}
