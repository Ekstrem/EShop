namespace DiscountCode.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IRedemption : IValueObject
{
    Guid OrderId { get; }
    DateTime RedeemedAt { get; }
}
