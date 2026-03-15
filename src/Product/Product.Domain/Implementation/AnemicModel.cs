namespace Product.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Product.Domain.Abstraction;

internal sealed class AnemicModel : IProductAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IProductRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IProductVariant> Variants { get; internal set; } = new List<IProductVariant>();
    public IReadOnlyList<IProductMedia> Media { get; internal set; } = new List<IProductMedia>();

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
