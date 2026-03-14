namespace Campaign.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICampaignRoot : IAggregateRoot<ICampaign>
{
    string Name { get; }
    string Subject { get; }
    string TemplateId { get; }
    string SegmentId { get; }
    DateTime? ScheduledAt { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
