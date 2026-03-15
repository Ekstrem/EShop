namespace Invoice.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using Invoice.Domain.Abstraction;

internal sealed class AnemicModel : IInvoiceAnemicModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long Version { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public string CommandName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Guid CorrelationToken { get; set; } = Guid.NewGuid();
    public IInvoiceRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IInvoiceLine> Lines { get; internal set; } = new List<IInvoiceLine>();
    public IInvoiceTotals Totals { get; internal set; } = null!;

    public IDictionary<string, IValueObject> Invariants => GetValueObjects();
    public IDictionary<string, IValueObject> GetValueObjects() =>
        new Dictionary<string, IValueObject> { ["Root"] = Root };
}
