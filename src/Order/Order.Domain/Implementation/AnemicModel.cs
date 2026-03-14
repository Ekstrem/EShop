namespace Order.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IOrder>, IOrderAnemicModel
{
    public IOrderRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IOrderLine> Lines { get; internal set; } = new List<IOrderLine>();
    public IOrderTotal OrderTotal { get; internal set; } = null!;
}
