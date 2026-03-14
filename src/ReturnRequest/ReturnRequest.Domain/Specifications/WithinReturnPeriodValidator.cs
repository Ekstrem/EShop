using Hive.SeedWorks.TacticalPatterns;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is within the 14-day return period.
/// </summary>
public sealed class WithinReturnPeriodValidator : IBusinessOperationValidator<IReturnRequest, IReturnRequestAnemicModel>
{
    private const int ReturnPeriodDays = 14;
    private readonly DateTime _orderDeliveredAt;

    public WithinReturnPeriodValidator(DateTime orderDeliveredAt)
    {
        _orderDeliveredAt = orderDeliveredAt;
    }

    public bool IsValid(IReturnRequestAnemicModel model)
    {
        var daysSinceDelivery = (model.Root.RequestedAt - _orderDeliveredAt).TotalDays;
        return daysSinceDelivery <= ReturnPeriodDays;
    }

    public string ErrorMessage =>
        $"Return request must be made within {ReturnPeriodDays} days of delivery.";
}
