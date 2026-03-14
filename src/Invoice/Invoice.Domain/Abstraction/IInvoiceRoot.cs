namespace Invoice.Domain.Abstraction;

using Hive.SeedWorks.TacticalPatterns;

public interface IInvoiceRoot : IAggregateRoot<IInvoice>
{
    string InvoiceNumber { get; }
    Guid OrderId { get; }
    Guid CustomerId { get; }
    DateTime IssueDate { get; }
    string Status { get; }
    string InvoiceType { get; }
}
