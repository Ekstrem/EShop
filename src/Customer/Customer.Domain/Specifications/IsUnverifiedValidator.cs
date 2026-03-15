namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the customer status is Unverified.
/// </summary>
public sealed class IsUnverifiedValidator
{
    private IsUnverifiedValidator() { }

    public static IsUnverifiedValidator CreateInstance()
    {
        return new IsUnverifiedValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status == "Unverified";
    }

    public string Reason => "Customer must be in Unverified status.";
}
