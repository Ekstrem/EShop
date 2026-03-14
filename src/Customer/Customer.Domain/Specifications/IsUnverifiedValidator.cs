using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the customer status is Unverified.
/// </summary>
public sealed class IsUnverifiedValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
{
    private IsUnverifiedValidator()
    {
    }

    public static IsUnverifiedValidator CreateInstance()
    {
        return new IsUnverifiedValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status == "Unverified";
    }

    public string ErrorMessage => "Customer must be in Unverified status.";
}
