namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class IsInitiatedValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Initiated";

    public string Reason => "Payment must be in Initiated status.";
}
