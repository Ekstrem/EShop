namespace Campaign.InternalContracts;

public sealed class CampaignReadModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string TemplateId { get; init; } = string.Empty;
    public string SegmentId { get; init; } = string.Empty;
    public DateTime? ScheduledAt { get; init; }
    public string Status { get; init; } = string.Empty;
    public int TotalRecipients { get; init; }
    public int SentCount { get; init; }
    public int FailedCount { get; init; }
    public DateTime CreatedAt { get; init; }
}
