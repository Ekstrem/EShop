namespace Invoice.InternalContracts;

public sealed class InvoiceReadModel
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime IssueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string InvoiceType { get; set; } = string.Empty;
    public decimal SubTotal { get; set; }
    public decimal VatTotal { get; set; }
    public decimal GrandTotal { get; set; }
    public IReadOnlyList<InvoiceLineReadModel> Lines { get; set; } = new List<InvoiceLineReadModel>();
}

public sealed class InvoiceLineReadModel
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal LineTotal { get; set; }
}
