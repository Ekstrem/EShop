namespace DiscountCode.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IDiscountCodeRoot : IAggregateRoot<IDiscountCode>
{
    string Code { get; }
    Guid? PromotionId { get; }
    int MaxUsage { get; }
    int UsageCount { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    DateTime? ExpiresAt { get; }
}
