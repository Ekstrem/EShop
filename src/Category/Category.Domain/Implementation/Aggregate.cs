namespace Category.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Category.Domain.Abstraction;
using Category.Domain.Specifications;

internal sealed class Aggregate : Aggregate<ICategory, ICategoryAnemicModel>
{
    private Aggregate(ICategoryAnemicModel model) : base(model) { }

    public static Aggregate CreateInstance(ICategoryAnemicModel model) => new(model);

    public AggregateResult<ICategory, ICategoryAnemicModel> CreateCategory(
        string name,
        Guid? parentId,
        int depth,
        int sortOrder,
        IReadOnlyList<string> siblingNames)
    {
        var depthValidator = new MaxDepthValidator();
        if (!depthValidator.IsSatisfiedBy(depth))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category depth must not exceed 4.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(siblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(name))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category name must be unique among siblings.");

        var root = CategoryRoot.CreateInstance(name, parentId, depth, sortOrder);
        var anemic = new AnemicModel { Root = root };

        return AggregateResult<ICategory, ICategoryAnemicModel>.Ok(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> UpdateCategory(
        string name,
        int sortOrder,
        IReadOnlyList<string> siblingNames)
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Only active categories can be updated.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(siblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(name))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category name must be unique among siblings.");

        var root = CategoryRoot.CreateInstance(
            name,
            Model.Root.ParentId,
            Model.Root.Depth,
            sortOrder,
            Model.Root.Status);

        var anemic = new AnemicModel { Root = root };
        return AggregateResult<ICategory, ICategoryAnemicModel>.Ok(anemic);
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
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Only active categories can be moved.");

        var depthValidator = new MaxDepthValidator();
        if (!depthValidator.IsSatisfiedBy(newDepth))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category depth must not exceed 4.");

        var uniqueNameValidator = new UniqueNameAmongSiblingsValidator(newSiblingNames);
        if (!uniqueNameValidator.IsSatisfiedBy(Model.Root.Name))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category name must be unique among siblings at the target location.");

        var noCycleValidator = new NoCycleValidator(ancestorIds);
        if (!noCycleValidator.IsSatisfiedBy(Model))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Moving would create a cycle in the category hierarchy.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            newParentId,
            newDepth,
            sortOrder,
            Model.Root.Status);

        var anemic = new AnemicModel { Root = root };
        return AggregateResult<ICategory, ICategoryAnemicModel>.Ok(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> DeactivateCategory(
        bool hasActiveChildren)
    {
        var activeValidator = new IsActiveValidator();
        if (!activeValidator.IsSatisfiedBy(Model))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category is already inactive.");

        var noActiveChildrenValidator = new NoActiveChildrenValidator();
        if (!noActiveChildrenValidator.IsSatisfiedBy(hasActiveChildren))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Cannot deactivate a category with active children.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.ParentId,
            Model.Root.Depth,
            Model.Root.SortOrder,
            "Inactive");

        var anemic = new AnemicModel { Root = root };
        return AggregateResult<ICategory, ICategoryAnemicModel>.Ok(anemic);
    }

    public AggregateResult<ICategory, ICategoryAnemicModel> ActivateCategory()
    {
        var inactiveValidator = new IsInactiveValidator();
        if (!inactiveValidator.IsSatisfiedBy(Model))
            return AggregateResult<ICategory, ICategoryAnemicModel>.Fail("Category is already active.");

        var root = CategoryRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.ParentId,
            Model.Root.Depth,
            Model.Root.SortOrder,
            "Active");

        var anemic = new AnemicModel { Root = root };
        return AggregateResult<ICategory, ICategoryAnemicModel>.Ok(anemic);
    }
}
