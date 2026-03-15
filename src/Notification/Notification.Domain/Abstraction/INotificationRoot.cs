namespace Notification.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface INotificationRoot : IValueObject
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
