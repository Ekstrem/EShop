namespace StockItem.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options)
        : base(options)
    {
    }

    public DbSet<StockItemEntity> StockItems => Set<StockItemEntity>();
    public DbSet<ReservationEntity> Reservations => Set<ReservationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockItemEntity>(entity =>
        {
            entity.ToTable("StockItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).HasMaxLength(32).IsRequired();
            entity.HasIndex(e => new { e.VariantId, e.WarehouseId }).IsUnique();
            entity.HasMany(e => e.Reservations).WithOne().HasForeignKey(r => r.StockItemId);
        });

        modelBuilder.Entity<ReservationEntity>(entity =>
        {
            entity.ToTable("Reservations");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.StockItemId, e.OrderId }).IsUnique();
        });
    }
}

public sealed class StockItemEntity
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Total { get; set; }
    public int Reserved { get; set; }
    public int LowStockThreshold { get; set; }
    public string Status { get; set; } = "InStock";
    public List<ReservationEntity> Reservations { get; set; } = new();
}

public sealed class ReservationEntity
{
    public Guid Id { get; set; }
    public Guid StockItemId { get; set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
}
