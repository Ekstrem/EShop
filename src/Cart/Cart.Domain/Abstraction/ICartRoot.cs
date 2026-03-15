namespace Cart.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface ICartRoot : IValueObject
{
    Guid CustomerId { get; }
    Guid SessionId { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
