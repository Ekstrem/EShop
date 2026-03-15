namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the email address is not already taken by another customer.
/// </summary>
public sealed class EmailNotTakenValidator
{
    private readonly Func<string, bool> _emailExistsCheck;

    private EmailNotTakenValidator(Func<string, bool> emailExistsCheck)
    {
        _emailExistsCheck = emailExistsCheck;
    }

    public static EmailNotTakenValidator CreateInstance(Func<string, bool> emailExistsCheck)
    {
        return new EmailNotTakenValidator(emailExistsCheck);
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return !_emailExistsCheck(model.Root.Email);
    }

    public string Reason => "The email address is already registered.";
}
