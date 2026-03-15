namespace Campaign.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICampaignRoot : IValueObject
{
    string Name { get; }
    string Subject { get; }
    string TemplateId { get; }
    string SegmentId { get; }
    DateTime? ScheduledAt { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
