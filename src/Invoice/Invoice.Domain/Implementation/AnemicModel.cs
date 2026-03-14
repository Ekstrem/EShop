namespace Invoice.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Invoice.Domain.Abstraction;

internal sealed class AnemicModel : AnemicModel<IInvoice>, IInvoiceAnemicModel
{
    public IInvoiceRoot Root { get; internal set; } = null!;
    public IReadOnlyList<IInvoiceLine> Lines { get; internal set; } = new List<IInvoiceLine>();
    public IInvoiceTotals Totals { get; internal set; } = null!;
}
