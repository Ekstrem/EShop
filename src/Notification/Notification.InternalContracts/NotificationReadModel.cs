namespace Notification.InternalContracts;

public sealed class NotificationReadModel
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid EventId { get; init; }
    public string Channel { get; init; } = string.Empty;
    public string TemplateId { get; init; } = string.Empty;
    public string Locale { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public int RetryCount { get; init; }
    public string RenderedContent { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
