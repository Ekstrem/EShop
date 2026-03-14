namespace Notification.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class MaxRetryValidator : IBusinessOperationValidator<INotificationAnemicModel>
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.RetryCount < 3;

    public string ErrorMessage => "Maximum retry count (3) exceeded.";
}
