namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class CanVoidValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Initiated";

    public string Reason => "Only payments in Initiated status can be voided.";
}
