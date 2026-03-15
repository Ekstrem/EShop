namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class MaxRetryValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.RetryCount < 3;

    public string Reason => "Maximum retry count (3) exceeded.";
}
