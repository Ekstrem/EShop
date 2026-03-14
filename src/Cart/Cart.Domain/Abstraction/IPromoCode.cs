namespace Cart.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IPromoCode : IValueObject
{
    string Code { get; }
    decimal DiscountPercent { get; }
    decimal DiscountAmount { get; }
}
