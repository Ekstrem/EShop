namespace Notification.Domain.Specifications;

using Notification.Domain.Abstraction;

internal sealed class DeduplicationValidator
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.EventId != Guid.Empty && model.Root.CustomerId != Guid.Empty;

    public string Reason => "Notification for this eventId and customerId already exists.";
}
