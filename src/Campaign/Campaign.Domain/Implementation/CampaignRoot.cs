namespace Campaign.Domain.Implementation;

using Campaign.Domain.Abstraction;

internal sealed class CampaignRoot : ICampaignRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string TemplateId { get; private set; } = string.Empty;
    public string SegmentId { get; private set; } = string.Empty;
    public DateTime? ScheduledAt { get; private set; }
    public string Status { get; private set; } = "Draft";
    public DateTime CreatedAt { get; private set; }

    private CampaignRoot() { }

    public static ICampaignRoot CreateInstance(
        string name,
        string subject,
        string templateId,
        string segmentId,
        DateTime? scheduledAt = null,
        string status = "Draft",
        DateTime? createdAt = null)
        => new CampaignRoot
        {
            Name = name,
            Subject = subject,
            TemplateId = templateId,
            SegmentId = segmentId,
            ScheduledAt = scheduledAt,
            Status = status,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
}
