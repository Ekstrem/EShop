namespace Promotion.InternalContracts;

public sealed class PromotionReadModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string DiscountType { get; init; } = string.Empty;
    public decimal DiscountValue { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Conditions { get; init; } = string.Empty;
    public bool AllowStacking { get; init; }
}
