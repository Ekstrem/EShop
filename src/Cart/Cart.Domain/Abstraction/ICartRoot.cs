namespace Cart.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface ICartRoot : IAggregateRoot<ICart>
{
    Guid CustomerId { get; }
    Guid SessionId { get; }
    string Status { get; }
    DateTime CreatedAt { get; }
}
