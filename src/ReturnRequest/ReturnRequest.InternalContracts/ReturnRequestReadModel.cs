namespace ReturnRequest.InternalContracts;

/// <summary>
/// Read model for return request queries.
/// </summary>
public sealed class ReturnRequestReadModel
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public string RmaNumber { get; init; } = string.Empty;
    public string Reason { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime RequestedAt { get; init; }
    public decimal RefundAmount { get; init; }
    public IReadOnlyList<ReturnItemReadModel> Items { get; init; } = new List<ReturnItemReadModel>();
    public ReturnLabelReadModel? ReturnLabel { get; init; }
}

/// <summary>
/// Read model for a return item.
/// </summary>
public sealed class ReturnItemReadModel
{
    public Guid VariantId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}

/// <summary>
/// Read model for a return label.
/// </summary>
public sealed class ReturnLabelReadModel
{
    public string LabelUrl { get; init; } = string.Empty;
    public string Carrier { get; init; } = string.Empty;
}
