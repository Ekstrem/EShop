namespace DiscountCode.Domain.Implementation;

using DiscountCode.Domain.Abstraction;

internal sealed class DiscountCodeRoot : IDiscountCodeRoot
{
    public string Code { get; private set; } = string.Empty;
    public Guid? PromotionId { get; private set; }
    public int MaxUsage { get; private set; }
    public int UsageCount { get; private set; }
    public string Status { get; private set; } = "Active";
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private DiscountCodeRoot() { }

    public static IDiscountCodeRoot CreateInstance(
        string code,
        Guid? promotionId,
        int maxUsage,
        int usageCount = 0,
        string status = "Active",
        DateTime? createdAt = null,
        DateTime? expiresAt = null)
        => new DiscountCodeRoot
        {
            Code = code,
            PromotionId = promotionId,
            MaxUsage = maxUsage,
            UsageCount = usageCount,
            Status = status,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            ExpiresAt = expiresAt
        };
}
