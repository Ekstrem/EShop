namespace Invoice.Domain.Implementation;

using Invoice.Domain.Abstraction;

internal sealed class InvoiceLine : IInvoiceLine
{
    public string Description { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal VatRate { get; private set; }
    public decimal LineTotal { get; private set; }

    private InvoiceLine() { }

    public static IInvoiceLine CreateInstance(
        string description,
        int quantity,
        decimal unitPrice,
        decimal vatRate)
    {
        var lineTotal = quantity * unitPrice;
        return new InvoiceLine
        {
            Description = description,
            Quantity = quantity,
            UnitPrice = unitPrice,
            VatRate = vatRate,
            LineTotal = lineTotal
        };
    }
}
