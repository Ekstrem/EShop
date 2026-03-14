namespace Payment.Storage;

using Microsoft.EntityFrameworkCore;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentEntity>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderId).IsRequired();
            entity.HasIndex(e => e.OrderId).IsUnique();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Currency).HasMaxLength(3).IsRequired();
            entity.Property(e => e.PaymentMethod).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(30).IsRequired();
            entity.HasMany(e => e.Transactions)
                .WithOne()
                .HasForeignKey(t => t.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TransactionEntity>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProviderTransactionId).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => e.ProviderTransactionId).IsUnique();
            entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasMaxLength(30).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}

public sealed class PaymentEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<TransactionEntity> Transactions { get; set; } = new();
}

public sealed class TransactionEntity
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public string ProviderTransactionId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
