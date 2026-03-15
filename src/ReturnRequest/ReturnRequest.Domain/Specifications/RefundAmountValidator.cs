using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the refund amount does not exceed the total cost of returned items.
/// </summary>
internal sealed class RefundAmountValidator
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
    {
        var totalItemsCost = model.Items.Sum(item => item.Quantity * item.UnitPrice);
        return model.RefundAmount >= 0 && model.RefundAmount <= totalItemsCost;
    }

    public string Reason =>
        "Refund amount must not exceed the total cost of returned items.";
}
