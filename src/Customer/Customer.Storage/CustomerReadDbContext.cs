using Customer.InternalContracts;
using Microsoft.EntityFrameworkCore;

namespace Customer.Storage;

/// <summary>
/// Read-side DbContext for Customer query projections.
/// </summary>
public sealed class CustomerReadDbContext : DbContext, ICustomerQueryRepository
{
    public CustomerReadDbContext(DbContextOptions<CustomerReadDbContext> options) : base(options)
    {
    }

    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>(e =>
        {
            e.ToTable("Customers");
            e.HasKey(c => c.Id);
            e.HasMany(c => c.Addresses).WithOne().HasForeignKey(a => a.CustomerId);
            e.HasMany(c => c.Consents).WithOne().HasForeignKey(cn => cn.CustomerId);
        });

        modelBuilder.Entity<AddressEntity>(e =>
        {
            e.ToTable("CustomerAddresses");
            e.HasKey(a => a.Id);
        });

        modelBuilder.Entity<ConsentEntity>(e =>
        {
            e.ToTable("CustomerConsents");
            e.HasKey(c => c.Id);
        });
    }

    public async Task<CustomerReadModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await Customers
            .AsNoTracking()
            .Include(c => c.Addresses)
            .Include(c => c.Consents)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<CustomerReadModel?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var entity = await Customers
            .AsNoTracking()
            .Include(c => c.Addresses)
            .Include(c => c.Consents)
            .FirstOrDefaultAsync(c => c.Email == email, ct);

        return entity is null ? null : MapToReadModel(entity);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
    {
        return await Customers.AsNoTracking().AnyAsync(c => c.Email == email, ct);
    }

    private static CustomerReadModel MapToReadModel(CustomerEntity entity)
    {
        return new CustomerReadModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Status = entity.Status,
            Addresses = entity.Addresses.Select(a => new AddressReadModel
            {
                Street = a.Street,
                City = a.City,
                ZipCode = a.ZipCode,
                Country = a.Country,
                IsDefault = a.IsDefault
            }).ToList(),
            Consents = entity.Consents.Select(c => new ConsentReadModel
            {
                ConsentType = c.ConsentType,
                IsGranted = c.IsGranted,
                GrantedAt = c.GrantedAt
            }).ToList()
        };
    }
}
