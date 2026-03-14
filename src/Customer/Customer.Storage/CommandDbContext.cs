using Microsoft.EntityFrameworkCore;

namespace Customer.Storage;

/// <summary>
/// EF Core DbContext for Customer command-side persistence.
/// </summary>
public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<AddressEntity> Addresses => Set<AddressEntity>();
    public DbSet<ConsentEntity> Consents => Set<ConsentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(e =>
        {
            e.ToTable("Customers");
            e.HasKey(c => c.Id);
            e.Property(c => c.Email).HasMaxLength(256).IsRequired();
            e.HasIndex(c => c.Email).IsUnique();
            e.Property(c => c.FirstName).HasMaxLength(128).IsRequired();
            e.Property(c => c.LastName).HasMaxLength(128).IsRequired();
            e.Property(c => c.PasswordHash).HasMaxLength(512).IsRequired();
            e.Property(c => c.Status).HasMaxLength(32).IsRequired();
            e.HasMany(c => c.Addresses).WithOne().HasForeignKey(a => a.CustomerId);
            e.HasMany(c => c.Consents).WithOne().HasForeignKey(cn => cn.CustomerId);
        });

        modelBuilder.Entity<AddressEntity>(e =>
        {
            e.ToTable("CustomerAddresses");
            e.HasKey(a => a.Id);
            e.Property(a => a.Street).HasMaxLength(256).IsRequired();
            e.Property(a => a.City).HasMaxLength(128).IsRequired();
            e.Property(a => a.ZipCode).HasMaxLength(20).IsRequired();
            e.Property(a => a.Country).HasMaxLength(64).IsRequired();
        });

        modelBuilder.Entity<ConsentEntity>(e =>
        {
            e.ToTable("CustomerConsents");
            e.HasKey(c => c.Id);
            e.Property(c => c.ConsentType).HasMaxLength(64).IsRequired();
        });
    }
}

public sealed class CustomerEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<AddressEntity> Addresses { get; set; } = new();
    public List<ConsentEntity> Consents { get; set; } = new();
}

public sealed class AddressEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}

public sealed class ConsentEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string ConsentType { get; set; } = string.Empty;
    public bool IsGranted { get; set; }
    public DateTime GrantedAt { get; set; }
}
