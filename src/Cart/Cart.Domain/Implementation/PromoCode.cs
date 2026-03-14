namespace Cart.Domain.Implementation;

using Cart.Domain.Abstraction;

internal sealed class PromoCode : IPromoCode
{
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercent { get; private set; }
    public decimal DiscountAmount { get; private set; }

    private PromoCode() { }

    public static IPromoCode CreateInstance(
        string code,
        decimal discountPercent,
        decimal discountAmount)
        => new PromoCode
        {
            Code = code,
            DiscountPercent = discountPercent,
            DiscountAmount = discountAmount
        };
}
