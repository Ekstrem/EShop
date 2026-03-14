namespace Invoice.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<InvoiceLineEntity> InvoiceLines => Set<InvoiceLineEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceEntity>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.Property(e => e.OrderId).IsRequired();
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.IssueDate).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
            entity.Property(e => e.InvoiceType).HasMaxLength(20).IsRequired();
            entity.Property(e => e.SubTotal).HasPrecision(18, 2);
            entity.Property(e => e.VatTotal).HasPrecision(18, 2);
            entity.Property(e => e.GrandTotal).HasPrecision(18, 2);
            entity.HasMany(e => e.Lines)
                .WithOne()
                .HasForeignKey(l => l.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InvoiceLineEntity>(entity =>
        {
            entity.ToTable("InvoiceLines");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.VatRate).HasPrecision(5, 2);
            entity.Property(e => e.LineTotal).HasPrecision(18, 2);
        });
    }
}

public sealed class InvoiceEntity
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string InvoiceType { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal VatTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public List<InvoiceLineEntity> Lines { get; set; } = new();
}

public sealed class InvoiceLineEntity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal LineTotal { get; set; }
}
