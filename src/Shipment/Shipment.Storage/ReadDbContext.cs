using Microsoft.EntityFrameworkCore;
using Shipment.Storage.Entities;

namespace Shipment.Storage;

/// <summary>
/// Read-only database context for the Shipment bounded context.
/// </summary>
public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
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
            entity.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.ShipmentId);
        });

        modelBuilder.Entity<ShipmentItemEntity>(entity =>
        {
            entity.ToTable("ShipmentItems");
            entity.HasKey(e => e.Id);
        });
    }
}
