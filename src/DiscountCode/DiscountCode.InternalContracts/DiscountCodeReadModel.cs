namespace DiscountCode.InternalContracts;

public sealed class DiscountCodeReadModel
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public Guid? PromotionId { get; init; }
    public int MaxUsage { get; init; }
    public int UsageCount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public IReadOnlyList<RedemptionReadModel> Redemptions { get; init; } = new List<RedemptionReadModel>();
}

public sealed class RedemptionReadModel
{
    public Guid OrderId { get; init; }
    public DateTime RedeemedAt { get; init; }
}
