using Customer.Domain;
using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Hive.SeedWorks.TacticalPatterns;
using Microsoft.EntityFrameworkCore;

namespace Customer.Storage;

/// <summary>
/// Repository implementation for Customer aggregate persistence.
/// </summary>
public sealed class CustomerRepository : IRepository<ICustomer, ICustomerAnemicModel>
{
    private readonly CommandDbContext _dbContext;

    public CustomerRepository(CommandDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICustomerAnemicModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _dbContext.Customers
            .Include(c => c.Addresses)
            .Include(c => c.Consents)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (entity is null) return null;

        var root = CustomerRoot.CreateInstance(
            entity.Id, entity.Email, entity.FirstName,
            entity.LastName, entity.PasswordHash, entity.Status);

        var addresses = entity.Addresses
            .Select(a => (IAddress)Address.CreateInstance(
                a.Street, a.City, a.ZipCode, a.Country, a.IsDefault))
            .ToList()
            .AsReadOnly();

        var addressBook = AddressBook.CreateInstance(addresses);

        var consents = entity.Consents
            .Select(c => (IConsent)Consent.CreateInstance(
                c.ConsentType, c.IsGranted, c.GrantedAt))
            .ToList()
            .AsReadOnly();

        return CustomerAnemicModel.CreateInstance(entity.Id, root, addressBook, consents);
    }

    public async Task SaveAsync(ICustomerAnemicModel model, CancellationToken ct = default)
    {
        var existing = await _dbContext.Customers
            .Include(c => c.Addresses)
            .Include(c => c.Consents)
            .FirstOrDefaultAsync(c => c.Id == model.Id, ct);

        if (existing is null)
        {
            var entity = new CustomerEntity
            {
                Id = model.Id,
                Email = model.Root.Email,
                FirstName = model.Root.FirstName,
                LastName = model.Root.LastName,
                PasswordHash = model.Root.PasswordHash,
                Status = model.Root.Status,
                Addresses = model.AddressBook.Addresses.Select(a => new AddressEntity
                {
                    Id = Guid.NewGuid(),
                    CustomerId = model.Id,
                    Street = a.Street,
                    City = a.City,
                    ZipCode = a.ZipCode,
                    Country = a.Country,
                    IsDefault = a.IsDefault
                }).ToList(),
                Consents = model.Consents.Select(c => new ConsentEntity
                {
                    Id = Guid.NewGuid(),
                    CustomerId = model.Id,
                    ConsentType = c.ConsentType,
                    IsGranted = c.IsGranted,
                    GrantedAt = c.GrantedAt
                }).ToList()
            };
            _dbContext.Customers.Add(entity);
        }
        else
        {
            existing.Email = model.Root.Email;
            existing.FirstName = model.Root.FirstName;
            existing.LastName = model.Root.LastName;
            existing.PasswordHash = model.Root.PasswordHash;
            existing.Status = model.Root.Status;

            _dbContext.Addresses.RemoveRange(existing.Addresses);
            existing.Addresses = model.AddressBook.Addresses.Select(a => new AddressEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = model.Id,
                Street = a.Street,
                City = a.City,
                ZipCode = a.ZipCode,
                Country = a.Country,
                IsDefault = a.IsDefault
            }).ToList();

            _dbContext.Consents.RemoveRange(existing.Consents);
            existing.Consents = model.Consents.Select(c => new ConsentEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = model.Id,
                ConsentType = c.ConsentType,
                IsGranted = c.IsGranted,
                GrantedAt = c.GrantedAt
            }).ToList();
        }

        await _dbContext.SaveChangesAsync(ct);
    }
}
