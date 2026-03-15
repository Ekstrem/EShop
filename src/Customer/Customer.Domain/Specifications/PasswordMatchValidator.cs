namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the provided old password hash matches the current password hash.
/// </summary>
public sealed class PasswordMatchValidator
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

    public string Reason => "The current password is incorrect.";
}
