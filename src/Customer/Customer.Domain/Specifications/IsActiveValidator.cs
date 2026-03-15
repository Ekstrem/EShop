namespace Customer.Domain.Specifications;

using Customer.Domain.Abstraction;

/// <summary>
/// Validates that the customer status is Active.
/// </summary>
public sealed class IsActiveValidator
{
    private IsActiveValidator() { }

    public static IsActiveValidator CreateInstance()
    {
        return new IsActiveValidator();
    }

    public bool IsSatisfiedBy(ICustomerAnemicModel model)
    {
        return model.Root.Status == "Active";
    }

    public string Reason => "Customer must be in Active status.";
}
