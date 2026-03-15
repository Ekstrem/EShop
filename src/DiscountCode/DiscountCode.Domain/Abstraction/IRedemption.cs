namespace DiscountCode.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IRedemption : IValueObject
{
    Guid OrderId { get; }
    DateTime RedeemedAt { get; }
}
