namespace ReturnRequest.Storage.Entities;

/// <summary>
/// Persistence entity for the ReturnRequest aggregate.
/// </summary>
public sealed class ReturnRequestEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public string RmaNumber { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Requested";
    public DateTime RequestedAt { get; set; }
    public decimal RefundAmount { get; set; }
    public string? ReturnLabelUrl { get; set; }
    public string? ReturnLabelCarrier { get; set; }
    public List<ReturnItemEntity> Items { get; set; } = new();
}
