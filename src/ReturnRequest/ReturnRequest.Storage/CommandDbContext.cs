using Microsoft.EntityFrameworkCore;
using ReturnRequest.Storage.Entities;

namespace ReturnRequest.Storage;

/// <summary>
/// Command database context for the ReturnRequest bounded context.
/// </summary>
public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<ReturnRequestEntity> ReturnRequests => Set<ReturnRequestEntity>();
    public DbSet<ReturnItemEntity> ReturnItems => Set<ReturnItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReturnRequestEntity>(entity =>
        {
            entity.ToTable("ReturnRequests");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.RmaNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.RefundAmount).HasPrecision(18, 2);
            entity.Property(e => e.ReturnLabelUrl).HasMaxLength(1000);
            entity.Property(e => e.ReturnLabelCarrier).HasMaxLength(50);

            entity.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.ReturnRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReturnItemEntity>(entity =>
        {
            entity.ToTable("ReturnItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VariantId).IsRequired();
            entity.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2).IsRequired();
        });
    }
}
