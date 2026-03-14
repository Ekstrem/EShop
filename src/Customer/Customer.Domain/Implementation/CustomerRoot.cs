using Customer.Domain.Abstraction;

namespace Customer.Domain.Implementation;

/// <summary>
/// Immutable implementation of the Customer aggregate root.
/// </summary>
public sealed class CustomerRoot : ICustomerRoot
{
    private CustomerRoot(
        Guid id,
        string email,
        string firstName,
        string lastName,
        string passwordHash,
        string status)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
        Status = status;
    }

    public Guid Id { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PasswordHash { get; }
    public string Status { get; }

    public static CustomerRoot CreateInstance(
        Guid id,
        string email,
        string firstName,
        string lastName,
        string passwordHash,
        string status)
    {
        return new CustomerRoot(id, email, firstName, lastName, passwordHash, status);
    }
}
