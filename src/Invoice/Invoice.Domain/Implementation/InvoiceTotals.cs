namespace Invoice.Domain.Implementation;

using Invoice.Domain.Abstraction;

internal sealed class InvoiceTotals : IInvoiceTotals
{
    public decimal SubTotal { get; private set; }
    public decimal VatTotal { get; private set; }
    public decimal GrandTotal { get; private set; }

    private InvoiceTotals() { }

    public static IInvoiceTotals CreateInstance(
        decimal subTotal,
        decimal vatTotal,
        decimal grandTotal)
        => new InvoiceTotals
        {
            SubTotal = subTotal,
            VatTotal = vatTotal,
            GrandTotal = grandTotal
        };

    public static IInvoiceTotals CalculateFromLines(IReadOnlyList<IInvoiceLine> lines)
    {
        var subTotal = lines.Sum(l => l.LineTotal);
        var vatTotal = lines.Sum(l => l.LineTotal * l.VatRate / 100m);
        var grandTotal = subTotal + vatTotal;

        return new InvoiceTotals
        {
            SubTotal = subTotal,
            VatTotal = vatTotal,
            GrandTotal = grandTotal
        };
    }
}
