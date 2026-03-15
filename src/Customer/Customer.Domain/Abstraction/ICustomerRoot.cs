namespace Customer.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

/// <summary>
/// Aggregate root for the Customer bounded context.
/// </summary>
public interface ICustomerRoot : IValueObject
{
    Guid Id { get; }
    string Email { get; }
    string FirstName { get; }
    string LastName { get; }
    string PasswordHash { get; }
    string Status { get; }
}
