namespace StockItem.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class StockItemReadDbContext : DbContext
{
    public StockItemReadDbContext(DbContextOptions<StockItemReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<StockItemEntity> StockItems => Set<StockItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockItemEntity>(entity =>
        {
            entity.ToTable("StockItems");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Reservations).WithOne().HasForeignKey(r => r.StockItemId);
        });

        modelBuilder.Entity<ReservationEntity>(entity =>
        {
            entity.ToTable("Reservations");
            entity.HasKey(e => e.Id);
        });
    }
}
