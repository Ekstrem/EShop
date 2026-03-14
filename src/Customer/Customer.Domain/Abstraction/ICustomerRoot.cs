using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Abstraction;

/// <summary>
/// Aggregate root for the Customer bounded context.
/// </summary>
public interface ICustomerRoot : IAggregateRoot<ICustomer>
{
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    string PasswordHash { get; }
    string Status { get; }
}
