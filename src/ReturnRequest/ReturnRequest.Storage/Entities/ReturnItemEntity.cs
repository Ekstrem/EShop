namespace ReturnRequest.Storage.Entities;

/// <summary>
/// Persistence entity for a return item.
/// </summary>
public sealed class ReturnItemEntity
{
    public Guid Id { get; set; }
    public Guid ReturnRequestId { get; set; }
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
