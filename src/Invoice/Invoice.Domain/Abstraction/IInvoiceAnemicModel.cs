namespace Invoice.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IInvoiceAnemicModel : IAnemicModel<IInvoice>
{
    IInvoiceRoot Root { get; }
    IReadOnlyList<IInvoiceLine> Lines { get; }
    IInvoiceTotals Totals { get; }
}
