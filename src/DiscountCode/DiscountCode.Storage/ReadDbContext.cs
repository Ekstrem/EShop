namespace DiscountCode.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<DiscountCodeEntity> DiscountCodes => Set<DiscountCodeEntity>();
    public DbSet<RedemptionEntity> Redemptions => Set<RedemptionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiscountCodeEntity>(entity =>
        {
            entity.ToTable("discount_codes");
            entity.HasKey(e => e.Id);
            entity.HasNoKey();
        });

        modelBuilder.Entity<RedemptionEntity>(entity =>
        {
            entity.ToTable("redemptions");
            entity.HasKey(e => e.Id);
            entity.HasNoKey();
        });
    }
}
