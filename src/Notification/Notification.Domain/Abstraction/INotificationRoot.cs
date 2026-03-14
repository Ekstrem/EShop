namespace Notification.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface INotificationRoot : IAggregateRoot<INotification>
{
    Guid CustomerId { get; }
    Guid EventId { get; }
    string Channel { get; }
    string TemplateId { get; }
    string Locale { get; }
    string Type { get; }
    string Status { get; }
    int RetryCount { get; }
    DateTime CreatedAt { get; }
}
