namespace Invoice.Domain.Abstraction;

using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

public interface IInvoiceRoot : IValueObject
{
    string InvoiceNumber { get; }
    Guid OrderId { get; }
    Guid CustomerId { get; }
    DateTime IssueDate { get; }
    string Status { get; }
    string InvoiceType { get; }
}
