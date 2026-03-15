namespace Promotion.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IPromotionRoot : IValueObject
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
