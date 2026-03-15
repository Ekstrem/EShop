namespace Order.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;

internal sealed class AnemicModel : IOrderAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IOrderRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IOrderLine> Lines { get; internal set; } = new List<IOrderLine>();
    public IOrderTotal OrderTotal { get; internal set; } = null!;

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject>
        {
            ["Root"] = Root,
            ["OrderTotal"] = OrderTotal
        };
}
