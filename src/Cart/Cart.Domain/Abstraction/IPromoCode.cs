namespace Cart.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IPromoCode : IValueObject
{
    string Code { get; }
    decimal DiscountPercent { get; }
    decimal DiscountAmount { get; }
}
