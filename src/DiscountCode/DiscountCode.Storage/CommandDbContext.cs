namespace DiscountCode.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<DiscountCodeEntity> DiscountCodes => Set<DiscountCodeEntity>();
    public DbSet<RedemptionEntity> Redemptions => Set<RedemptionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiscountCodeEntity>(entity =>
        {
            entity.ToTable("discount_codes");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.HasIndex(e => e.PromotionId);
            entity.HasIndex(e => e.Status);
        });

        modelBuilder.Entity<RedemptionEntity>(entity =>
        {
            entity.ToTable("redemptions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DiscountCodeId);
            entity.HasIndex(e => e.OrderId);
        });
    }
}

public sealed class DiscountCodeEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public Guid? PromotionId { get; set; }
    public int MaxUsage { get; set; }
    public int UsageCount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public ICollection<RedemptionEntity> Redemptions { get; set; } = new List<RedemptionEntity>();
}

public sealed class RedemptionEntity
{
    public Guid Id { get; set; }
    public Guid DiscountCodeId { get; set; }
    public Guid OrderId { get; set; }
    public DateTime RedeemedAt { get; set; }
}
