namespace Promotion.Domain.Implementation;

using Promotion.Domain.Abstraction;

internal sealed class PromotionRoot : IPromotionRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string DiscountType { get; private set; } = string.Empty;
    public decimal DiscountValue { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Status { get; private set; } = "Draft";
    public string Conditions { get; private set; } = string.Empty;

    private PromotionRoot() { }

    public static IPromotionRoot CreateInstance(
        string name,
        string description,
        string discountType,
        decimal discountValue,
        DateTime startDate,
        DateTime endDate,
        string status = "Draft",
        string conditions = "")
        => new PromotionRoot
        {
            Name = name,
            Description = description,
            DiscountType = discountType,
            DiscountValue = discountValue,
            StartDate = startDate,
            EndDate = endDate,
            Status = status,
            Conditions = conditions
        };
}
