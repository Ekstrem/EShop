namespace Promotion.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<PromotionEntity> Promotions => Set<PromotionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PromotionEntity>(entity =>
        {
            entity.ToTable("promotions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DiscountType);
            entity.HasIndex(e => new { e.StartDate, e.EndDate });
        });
    }
}

public sealed class PromotionEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Conditions { get; set; } = string.Empty;
    public bool AllowStacking { get; set; }
}
