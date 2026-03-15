namespace Notification.DomainServices;

using Notification.Domain.Abstraction;

public sealed class AggregateProvider
{
    public object Create(INotificationAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
