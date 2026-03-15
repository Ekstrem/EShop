namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the customer account has not been deactivated.
/// </summary>
public sealed class IsNotDeactivatedValidator
{
    private IsNotDeactivatedValidator() { }

    public static IsNotDeactivatedValidator CreateInstance()
    {
        return new IsNotDeactivatedValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status != "Deactivated";
    }

    public string Reason => "Customer account has been deactivated.";
}
