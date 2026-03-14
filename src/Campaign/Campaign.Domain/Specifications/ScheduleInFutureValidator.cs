namespace Campaign.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Campaign.Domain.Abstraction;

internal sealed class ScheduleInFutureValidator : IBusinessOperationValidator<ICampaignAnemicModel>
{
    private readonly DateTime _scheduledAt;

    public ScheduleInFutureValidator(DateTime scheduledAt)
    {
        _scheduledAt = scheduledAt;
    }

    public bool IsSatisfiedBy(ICampaignAnemicModel model)
        => _scheduledAt > DateTime.UtcNow;

    public string ErrorMessage => "Scheduled date must be in the future.";
}
