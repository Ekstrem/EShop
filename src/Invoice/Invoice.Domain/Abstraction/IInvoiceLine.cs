namespace Invoice.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IInvoiceLine : IValueObject
{
    string Description { get; }
    int Quantity { get; }
    decimal UnitPrice { get; }
    decimal VatRate { get; }
    decimal LineTotal { get; }
}
