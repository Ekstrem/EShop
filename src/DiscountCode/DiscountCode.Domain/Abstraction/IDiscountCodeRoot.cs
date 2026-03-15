namespace DiscountCode.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IDiscountCodeRoot : IValueObject
{
    string Code { get; }
    Guid? PromotionId { get; }
    int MaxUsage { get; }
    int UsageCount { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
    DateTime? ExpiresAt { get; }
}
