using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the provided old password hash matches the current password hash.
/// </summary>
public sealed class PasswordMatchValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
{
    private readonly string _oldPasswordHash;

    private PasswordMatchValidator(string oldPasswordHash)
    {
        _oldPasswordHash = oldPasswordHash;
    }

    public static PasswordMatchValidator CreateInstance(string oldPasswordHash)
    {
        return new PasswordMatchValidator(oldPasswordHash);
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.PasswordHash == _oldPasswordHash;
    }

    public string ErrorMessage => "The current password is incorrect.";
}
