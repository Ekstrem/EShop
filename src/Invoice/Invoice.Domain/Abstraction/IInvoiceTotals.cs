namespace Invoice.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IInvoiceTotals : IValueObject
{
    decimal SubTotal { get; }
    decimal VatTotal { get; }
    decimal GrandTotal { get; }
}
