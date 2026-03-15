namespace Order.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IOrderAnemicModel : IAnemicModel<IOrder>
{
    IOrderRoot Root { get; }
    IReadOnlyList<IOrderLine> Lines { get; }
    IOrderTotal OrderTotal { get; }
}
