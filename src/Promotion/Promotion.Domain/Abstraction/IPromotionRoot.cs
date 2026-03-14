namespace Promotion.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IPromotionRoot : IAggregateRoot<IPromotion>
{
    string Name { get; }
    string Description { get; }
    string DiscountType { get; }
    decimal DiscountValue { get; }
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    string Status { get; }
    string Conditions { get; }
}
