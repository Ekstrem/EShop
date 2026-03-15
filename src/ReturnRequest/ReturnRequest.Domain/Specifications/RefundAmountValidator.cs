using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the refund amount does not exceed the total cost of returned items.
/// </summary>
public sealed class RefundAmountValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
    {
        var totalItemsCost = model.Items.Sum(item => item.Quantity * item.UnitPrice);
        return model.RefundAmount >= 0 && model.RefundAmount <= totalItemsCost;
    }

    public string ErrorMessage =>
        "Refund amount must not exceed the total cost of returned items.";
}
