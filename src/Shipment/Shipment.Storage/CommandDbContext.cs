using Microsoft.EntityFrameworkCore;
using Shipment.Storage.Entities;

namespace Shipment.Storage;

/// <summary>
/// Command database context for the Shipment bounded context.
/// </summary>
public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<ShipmentEntity> Shipments => Set<ShipmentEntity>();
    public DbSet<ShipmentItemEntity> ShipmentItems => Set<ShipmentItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShipmentEntity>(entity =>
        {
            entity.ToTable("Shipments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.Carrier).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ShippingAddress).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LabelUrl).HasMaxLength(1000);

            entity.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ShipmentItemEntity>(entity =>
        {
            entity.ToTable("ShipmentItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VariantId).IsRequired();
            entity.Property(e => e.ProductName).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
        });
    }
}
