namespace Invoice.Domain.Implementation;

using Invoice.Domain.Abstraction;

internal sealed class InvoiceRoot : IInvoiceRoot
{
    public string InvoiceNumber { get; private set; } = string.Empty;
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime IssueDate { get; private set; }
    public string Status { get; private set; } = "Generated";
    public string InvoiceType { get; private set; } = "Invoice";

    private InvoiceRoot() { }

    public static IInvoiceRoot CreateInstance(
        string invoiceNumber,
        Guid orderId,
        Guid customerId,
        DateTime issueDate,
        string status = "Generated",
        string invoiceType = "Invoice")
        => new InvoiceRoot
        {
            InvoiceNumber = invoiceNumber,
            OrderId = orderId,
            CustomerId = customerId,
            IssueDate = issueDate,
            Status = status,
            InvoiceType = invoiceType
        };
}
