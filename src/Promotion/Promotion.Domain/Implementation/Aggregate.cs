namespace Promotion.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Promotion.Domain.Abstraction;
using Promotion.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IPromotionAnemicModel Model { get; }

    private Aggregate(IPromotionAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IPromotionAnemicModel model) => new(model);

    private AggregateResult<IPromotion, IPromotionAnemicModel> Success(IPromotionAnemicModel newModel)
    {
        var data = BusinessOperationData<IPromotion, IPromotionAnemicModel>
            .Commit<IPromotion, IPromotionAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IPromotion, IPromotionAnemicModel>(data);
    }

    private AggregateResult<IPromotion, IPromotionAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IPromotion, IPromotionAnemicModel>
            .Commit<IPromotion, IPromotionAnemicModel>(Model, Model);
        return new AggregateResultException<IPromotion, IPromotionAnemicModel>(
            data, new FailedSpecification<IPromotion, IPromotionAnemicModel>(error));
    }

    public AggregateResult<IPromotion, IPromotionAnemicModel> CreatePromotion(
        string name,
        string description,
        string discountType,
        decimal discountValue,
        DateTime startDate,
        DateTime endDate,
        string conditions,
        bool allowStacking)
    {
        var root = PromotionRoot.CreateInstance(
            name, description, discountType, discountValue, startDate, endDate, "Draft", conditions);

        var anemic = new AnemicModel
        {
            Root = root,
            AllowStacking = allowStacking
        };

        var dateValidator = new DateRangeValidator();
        if (!dateValidator.IsSatisfiedBy(anemic))
            return Fail("Start date must be before end date.");

        var discountValidator = new DiscountValueValidator();
        if (!discountValidator.IsSatisfiedBy(anemic))
            return Fail("Discount value must be greater than 0 and percentage must not exceed 100.");

        return Success(anemic);
    }

    public AggregateResult<IPromotion, IPromotionAnemicModel> UpdatePromotion(
        string name,
        string description,
        string discountType,
        decimal discountValue,
        DateTime startDate,
        DateTime endDate,
        string conditions,
        bool allowStacking)
    {
        var draftValidator = new IsDraftValidator();
        var activeValidator = new IsActiveValidator();
        if (!draftValidator.IsSatisfiedBy(Model) && !activeValidator.IsSatisfiedBy(Model))
            return Fail("Only Draft or Active promotions can be updated.");

        var root = PromotionRoot.CreateInstance(
            name, description, discountType, discountValue, startDate, endDate,
            Model.Root.Status, conditions);

        var anemic = new AnemicModel
        {
            Root = root,
            AllowStacking = allowStacking
        };

        var dateValidator = new DateRangeValidator();
        if (!dateValidator.IsSatisfiedBy(anemic))
            return Fail("Start date must be before end date.");

        var discValidator = new DiscountValueValidator();
        if (!discValidator.IsSatisfiedBy(anemic))
            return Fail("Discount value must be greater than 0 and percentage must not exceed 100.");

        return Success(anemic);
    }

    public AggregateResult<IPromotion, IPromotionAnemicModel> ActivatePromotion()
    {
        var draftValidator = new IsDraftValidator();
        if (!draftValidator.IsSatisfiedBy(Model))
            return Fail("Only draft promotions can be activated.");

        var expiredValidator = new IsNotExpiredValidator();
        if (!expiredValidator.IsSatisfiedBy(Model))
            return Fail("Cannot activate an expired promotion.");

        var root = PromotionRoot.CreateInstance(
            Model.Root.Name, Model.Root.Description, Model.Root.DiscountType,
            Model.Root.DiscountValue, Model.Root.StartDate, Model.Root.EndDate,
            "Active", Model.Root.Conditions);

        var anemic = new AnemicModel
        {
            Root = root,
            AllowStacking = Model.AllowStacking
        };

        return Success(anemic);
    }

    public AggregateResult<IPromotion, IPromotionAnemicModel> DeactivatePromotion()
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Only active promotions can be deactivated.");

        var root = PromotionRoot.CreateInstance(
            Model.Root.Name, Model.Root.Description, Model.Root.DiscountType,
            Model.Root.DiscountValue, Model.Root.StartDate, Model.Root.EndDate,
            "Expired", Model.Root.Conditions);

        var anemic = new AnemicModel
        {
            Root = root,
            AllowStacking = Model.AllowStacking
        };

        return Success(anemic);
    }
}
