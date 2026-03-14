using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Domain.Implementation;

/// <summary>
/// Immutable implementation of the return item value object.
/// </summary>
internal sealed class ReturnItem : IReturnItem
{
    public Guid VariantId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    private ReturnItem() { }

    public static IReturnItem CreateInstance(
        Guid variantId,
        string productName,
        int quantity,
        decimal unitPrice)
        => new ReturnItem
        {
            VariantId = variantId,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        };
}
