namespace Invoice.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IInvoiceTotals : IValueObject
{
    decimal SubTotal { get; }
    decimal VatTotal { get; }
    decimal GrandTotal { get; }
}
