namespace Campaign.Domain.Specifications;

using Campaign.Domain.Abstraction;

internal sealed class ScheduleInFutureValidator
{
    private readonly DateTime _scheduledAt;

    public ScheduleInFutureValidator(DateTime scheduledAt)
    {
        _scheduledAt = scheduledAt;
    }

    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => _scheduledAt > DateTime.UtcNow;

    public string Reason => "Scheduled date must be in the future.";
}
