using Customer.Domain.Abstraction;
using Hive.SeedWorks.TacticalPatterns;

namespace Customer.Domain.Specifications;

/// <summary>
/// Validates that the customer account has not been deactivated.
/// </summary>
public sealed class IsNotDeactivatedValidator : IBusinessOperationValidator<ICustomer, ICustomerAnemicModel>
{
    private IsNotDeactivatedValidator()
    {
    }

    public static IsNotDeactivatedValidator CreateInstance()
    {
        return new IsNotDeactivatedValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status != "Deactivated";
    }

    public string ErrorMessage => "Customer account has been deactivated.";
}
