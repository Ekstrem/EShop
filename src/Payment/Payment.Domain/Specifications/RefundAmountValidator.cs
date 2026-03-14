namespace Payment.Domain.Specifications;

using Payment.Domain.Abstraction;

internal sealed class RefundAmountValidator
{
    public bool IsSatisfiedBy(IPaymentAnemicModel model, decimal requestedRefundAmount)
    {
        var remainingRefundable = model.Root.Amount - model.TotalRefunded;
        return requestedRefundAmount > 0 && requestedRefundAmount <= remainingRefundable;
    }

    public string ErrorMessage => "Refund amount exceeds the remaining refundable amount.";
}
