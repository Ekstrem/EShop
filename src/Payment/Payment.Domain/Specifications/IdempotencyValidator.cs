namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class IdempotencyValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model, Guid orderId)
        => model.Root is null || model.Root.OrderId != orderId;

    public string Reason => "A payment for this order already exists.";
}
