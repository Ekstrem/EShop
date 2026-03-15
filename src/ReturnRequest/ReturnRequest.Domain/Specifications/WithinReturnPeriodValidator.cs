using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Specifications;

/// <summary>
/// Validates that the return request is within the 14-day return period.
/// </summary>
internal sealed class WithinReturnPeriodValidator
{
    private const int ReturnPeriodDays = 14;
    private readonly DateTime _orderDeliveredAt;

    public WithinReturnPeriodValidator(DateTime orderDeliveredAt)
    {
        _orderDeliveredAt = orderDeliveredAt;
    }

    public bool IsSatisfiedBy(IReturnRequestAnemicModel model)
    {
        var daysSinceDelivery = (model.Root.RequestedAt - _orderDeliveredAt).TotalDays;
        return daysSinceDelivery <= ReturnPeriodDays;
    }

    public string Reason =>
        $"Return request must be made within {ReturnPeriodDays} days of delivery.";
}
