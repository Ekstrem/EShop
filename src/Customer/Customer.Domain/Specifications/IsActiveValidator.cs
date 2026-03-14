using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the customer status is Active.
/// </summary>
public sealed class IsActiveValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
{
    private IsActiveValidator()
    {
    }

    public static IsActiveValidator CreateInstance()
    {
        return new IsActiveValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status == "Active";
    }

    public string ErrorMessage => "Customer must be in Active status.";
}
