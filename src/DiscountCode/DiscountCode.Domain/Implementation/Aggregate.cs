namespace DiscountCode.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using DiscountCode.Domain.Abstraction;
using DiscountCode.Domain.Specifications;

internal sealed class Aggregate : Aggregate<IDiscountCode, IDiscountCodeAnemicModel>
{
    private Aggregate(IDiscountCodeAnemicModel model) : base(model) { }

    public static Aggregate CreateInstance(IDiscountCodeAnemicModel model) => new(model);

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> GenerateDiscountCode(
        string code,
        Guid? promotionId,
        int maxUsage,
        DateTime? expiresAt)
    {
        var root = DiscountCodeRoot.CreateInstance(code, promotionId, maxUsage, expiresAt: expiresAt);
        var anemic = new AnemicModel
        {
            Root = root,
            Redemptions = new List<IRedemption>()
        };

        var formatValidator = new CodeFormatValidator();
        if (!formatValidator.IsSatisfiedBy(anemic))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Code must be 6-20 characters, uppercase alphanumeric only.");

        var uniqueValidator = new CodeUniqueValidator();
        if (!uniqueValidator.IsSatisfiedBy(anemic))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Code must be unique.");

        return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> ValidateDiscountCode()
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code is not active.");

        var usageValidator = new UsageLimitValidator();
        if (!usageValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code usage limit has been reached.");

        var expiredValidator = new NotExpiredValidator();
        if (!expiredValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code has expired.");

        return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Ok(Model);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> RedeemDiscountCode(Guid orderId)
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code is not active.");

        var usageValidator = new UsageLimitValidator();
        if (!usageValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code usage limit has been reached.");

        var expiredValidator = new NotExpiredValidator();
        if (!expiredValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Discount code has expired.");

        var newUsageCount = Model.Root.UsageCount + 1;
        var newStatus = newUsageCount >= Model.Root.MaxUsage ? "Exhausted" : Model.Root.Status;

        var root = DiscountCodeRoot.CreateInstance(
            Model.Root.Code,
            Model.Root.PromotionId,
            Model.Root.MaxUsage,
            newUsageCount,
            newStatus,
            Model.Root.CreatedAt,
            Model.Root.ExpiresAt);

        var redemptions = Model.Redemptions.ToList();
        redemptions.Add(Redemption.CreateInstance(orderId));

        var anemic = new AnemicModel
        {
            Root = root,
            Redemptions = redemptions
        };

        return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> CompensateRedemption(Guid orderId)
    {
        var redemption = Model.Redemptions.FirstOrDefault(r => r.OrderId == orderId);
        if (redemption is null)
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                $"Redemption for order '{orderId}' not found.");

        var newUsageCount = Model.Root.UsageCount - 1;
        var newStatus = Model.Root.Status == "Exhausted" ? "Active" : Model.Root.Status;

        var root = DiscountCodeRoot.CreateInstance(
            Model.Root.Code,
            Model.Root.PromotionId,
            Model.Root.MaxUsage,
            newUsageCount,
            newStatus,
            Model.Root.CreatedAt,
            Model.Root.ExpiresAt);

        var redemptions = Model.Redemptions.Where(r => r.OrderId != orderId).ToList();

        var anemic = new AnemicModel
        {
            Root = root,
            Redemptions = redemptions
        };

        return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> DeactivateCode()
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Fail(
                "Only active codes can be deactivated.");

        var root = DiscountCodeRoot.CreateInstance(
            Model.Root.Code,
            Model.Root.PromotionId,
            Model.Root.MaxUsage,
            Model.Root.UsageCount,
            "Deactivated",
            Model.Root.CreatedAt,
            Model.Root.ExpiresAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Redemptions = Model.Redemptions.ToList()
        };

        return AggregateResult<IDiscountCode, IDiscountCodeAnemicModel>.Ok(anemic);
    }
}
