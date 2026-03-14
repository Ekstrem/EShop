namespace Invoice.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<InvoiceLineEntity> InvoiceLines => Set<InvoiceLineEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceEntity>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Lines)
                .WithOne()
                .HasForeignKey(l => l.InvoiceId);
        });

        modelBuilder.Entity<InvoiceLineEntity>(entity =>
        {
            entity.ToTable("InvoiceLines");
            entity.HasKey(e => e.Id);
        });
    }
}
