namespace Notification.Domain.Implementation;

using Notification.Domain.Abstraction;

internal sealed class NotificationRoot : INotificationRoot
{
    public Guid CustomerId { get; private set; }
    public Guid EventId { get; private set; }
    public string Channel { get; private set; } = string.Empty;
    public string TemplateId { get; private set; } = string.Empty;
    public string Locale { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string Status { get; private set; } = "Created";
    public int RetryCount { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private NotificationRoot() { }

    public static INotificationRoot CreateInstance(
        Guid customerId,
        Guid eventId,
        string channel,
        string templateId,
        string locale,
        string type,
        string status = "Created",
        int retryCount = 0,
        DateTime? createdAt = null)
        => new NotificationRoot
        {
            CustomerId = customerId,
            EventId = eventId,
            Channel = channel,
            TemplateId = templateId,
            Locale = locale,
            Type = type,
            Status = status,
            RetryCount = retryCount,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
}
