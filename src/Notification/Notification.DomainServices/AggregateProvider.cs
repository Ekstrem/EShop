namespace Notification.DomainServices;

using Hive.SeedWorks.TacticalPatterns;
using Notification.Domain;
using Notification.Domain.Abstraction;

public sealed class AggregateProvider : IAggregateProvider<INotification, INotificationAnemicModel>
{
    public IAggregate<INotification, INotificationAnemicModel> Create(INotificationAnemicModel model)
        => Domain.Implementation.Aggregate.CreateInstance(model);
}
