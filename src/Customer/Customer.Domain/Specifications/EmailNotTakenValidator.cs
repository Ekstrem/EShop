using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the email address is not already taken by another customer.
/// </summary>
public sealed class EmailNotTakenValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
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

    public string ErrorMessage => "The email address is already registered.";
}
