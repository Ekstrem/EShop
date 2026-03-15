namespace Category.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Category.Domain.Abstraction;
using Category.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public ICategoryAnemicModel Model { get; }

    private Aggregate(ICategoryAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(ICategoryAnemicModel model) => new(model);

    private AggregateResult<ICategory, ICategoryAnemicModel> Success(ICategoryAnemicModel newModel)
    {
        var data = BusinessOperationData<ICategory, ICategoryAnemicModel>
            .Commit<ICategory, ICategoryAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<ICategory, ICategoryAnemicModel>(data);
    }

    private AggregateResult<ICategory, ICategoryAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<ICategory, ICategoryAnemicModel>
            .Commit<ICategory, ICategoryAnemicModel>(Model, Model);
        return new AggregateResultException<ICategory, ICategoryAnemicModel>(
            data, new FailedSpecification<ICategory, ICategoryAnemicModel>(error));
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> CreateCategory(
        string name,
        Guid? parentId,
        int depth,
        int sortOrder,
        IReadOnlyList<string> siblingNames)
    {
        var depthValidator = new MaxDepthValidator();
        if (!depthValidator.IsSatisfiedBy(depth))
            return Fail("Category depth must not exceed 4.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(siblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(name))
            return Fail("Category name must be unique among siblings.");

        var root = CategoryRoot.CreateInstance(name, parentId, depth, sortOrder);
        var anemic = new AnemicModel { Root = root };

        return Success(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> UpdateCategory(
        string name,
        int sortOrder,
        IReadOnlyList<string> siblingNames)
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Only active categories can be updated.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(siblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(name))
            return Fail("Category name must be unique among siblings.");

        var root = CategoryRoot.CreateInstance(
            name,
            Model.Root.ParentId,
            Model.Root.Depth,
            sortOrder,
            Model.Root.Status);

        var anemic = new AnemicModel { Root = root };
        return Success(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> MoveCategory(
        Guid? newParentId,
        int newDepth,
        int sortOrder,
        IReadOnlyList<string> newSiblingNames,
        IReadOnlyList<Guid> ancestorIds)
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Only active categories can be moved.");

        var depthValidator = new MaxDepthValidator();
        if (!depthValidator.IsSatisfiedBy(newDepth))
            return Fail("Category depth must not exceed 4.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(newSiblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(Model.Root.Name))
            return Fail("Category name must be unique among siblings at the target location.");

        var noCycleValidator = new NoCycleValidator(ancestorIds);
        if (!noCycleValidator.IsSatisfiedBy(Model))
            return Fail("Moving would create a cycle in the category hierarchy.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            newParentId,
            newDepth,
            sortOrder,
            Model.Root.Status);

        var anemic = new AnemicModel { Root = root };
        return Success(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> DeactivateCategory(
        bool hasActiveChildren)
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return Fail("Category is already inactive.");

        var noActiveChildrenValidator = new NoActiveChildrenValidator();
        if (!noActiveChildrenValidator.IsSatisfiedBy(hasActiveChildren))
            return Fail("Cannot deactivate a category with active children.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.ParentId,
            Model.Root.Depth,
            Model.Root.SortOrder,
            "Inactive");

        var anemic = new AnemicModel { Root = root };
        return Success(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> ActivateCategory()
    {
        var inactiveValidator = new IsInactiveValidator();
        if (!inactiveValidator.IsSatisfiedBy(Model))
            return Fail("Category is already active.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.ParentId,
            Model.Root.Depth,
            Model.Root.SortOrder,
            "Active");

        var anemic = new AnemicModel { Root = root };
        return Success(anemic);
    }
}
