namespace Notification.Domain.Specifications;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain.Abstraction;

internal sealed class DeduplicationValidator : IBusinessOperationValidator<INotificationAnemicModel>
{
    public bool IsSatisfiedBy(INotificationAnemicModel model)
        => model.Root.EventId != Guid.Empty && model.Root.CustomerId != Guid.Empty;

    public string ErrorMessage => "Notification for this eventId and customerId already exists.";
}
