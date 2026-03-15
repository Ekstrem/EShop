namespace DiscountCode.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using DiscountCode.Domain.Abstraction;
using DiscountCode.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IDiscountCodeAnemicModel Model { get; }

    private Aggregate(IDiscountCodeAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IDiscountCodeAnemicModel model) => new(model);

    private AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> Success(IDiscountCodeAnemicModel newModel)
    {
        var data = BusinessOperationData<IDiscountCode, IDiscountCodeAnemicModel>
            .Commit<IDiscountCode, IDiscountCodeAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IDiscountCode, IDiscountCodeAnemicModel>(data);
    }

    private AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IDiscountCode, IDiscountCodeAnemicModel>
            .Commit<IDiscountCode, IDiscountCodeAnemicModel>(Model, Model);
        return new AggregateResultException<IDiscountCode, IDiscountCodeAnemicModel>(
            data, new FailedSpecification<IDiscountCode, IDiscountCodeAnemicModel>(error));
    }

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
            return Fail("Code must be 6-20 characters, uppercase alphanumeric only.");

        var uniqueValidator = new CodeUniqueValidator();
        if (!uniqueValidator.IsSatisfiedBy(anemic))
            return Fail("Code must be unique.");

        return Success(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> ValidateDiscountCode()
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Discount code is not active.");

        var usageValidator = new UsageLimitValidator();
        if (!usageValidator.IsSatisfiedBy(Model))
            return Fail("Discount code usage limit has been reached.");

        var expiredValidator = new NotExpiredValidator();
        if (!expiredValidator.IsSatisfiedBy(Model))
            return Fail("Discount code has expired.");

        return Success(Model);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> RedeemDiscountCode(Guid orderId)
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Discount code is not active.");

        var usageValidator = new UsageLimitValidator();
        if (!usageValidator.IsSatisfiedBy(Model))
            return Fail("Discount code usage limit has been reached.");

        var expiredValidator = new NotExpiredValidator();
        if (!expiredValidator.IsSatisfiedBy(Model))
            return Fail("Discount code has expired.");

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

        return Success(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> CompensateRedemption(Guid orderId)
    {
        var redemption = Model.Redemptions.FirstOrDefault(r => r.OrderId == orderId);
        if (redemption is null)
            return Fail($"Redemption for order '{orderId}' not found.");

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

        return Success(anemic);
    }

    public AggregateResult<IDiscountCode, IDiscountCodeAnemicModel> DeactivateCode()
    {
        var activeValidator = new IsActiveCodeValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Only active codes can be deactivated.");

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

        return Success(anemic);
    }
}
