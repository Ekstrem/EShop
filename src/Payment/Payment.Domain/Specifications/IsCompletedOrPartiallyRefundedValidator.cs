namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class IsCompletedOrPartiallyRefundedValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model)
        => model.Root.Status == "Completed" || model.Root.Status == "PartiallyRefunded";

    public string Reason => "Payment must be in Completed or PartiallyRefunded status.";
}
