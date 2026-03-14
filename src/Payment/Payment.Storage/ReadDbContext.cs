namespace Payment.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentEntity>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Transactions)
                .WithOne()
                .HasForeignKey(t => t.PaymentId);
        });

        modelBuilder.Entity<TransactionEntity>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(e => e.Id);
        });
    }
}
