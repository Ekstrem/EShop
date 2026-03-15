namespace Product.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Product.Domain.Abstraction;
using Product.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IProductAnemicModel Model { get; }

    private Aggregate(IProductAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IProductAnemicModel model) => new(model);

    private AggregateResult<IProduct, IProductAnemicModel> Success(IProductAnemicModel newModel)
    {
        var data = BusinessOperationData<IProduct, IProductAnemicModel>
            .Commit<IProduct, IProductAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IProduct, IProductAnemicModel>(data);
    }

    private AggregateResult<IProduct, IProductAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IProduct, IProductAnemicModel>
            .Commit<IProduct, IProductAnemicModel>(Model, Model);
        return new AggregateResultException<IProduct, IProductAnemicModel>(
            data, new FailedSpecification<IProduct, IProductAnemicModel>(error));
    }

    public AggregateResult<IProduct, IProductAnemicModel> CreateProduct(
        string name,
        string description,
        Guid categoryId,
        IReadOnlyList<IProductVariant> variants,
        IReadOnlyList<IProductMedia> media)
    {
        var root = ProductRoot.CreateInstance(name, description, categoryId);
        var anemic = new AnemicModel
        {
            Root = root,
            Variants = variants.ToList(),
            Media = media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> UpdateProduct(
        string name,
        string description,
        Guid categoryId)
    {
        var validator = new IsNotArchivedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Cannot update an archived product.");

        var root = ProductRoot.CreateInstance(name, description, categoryId, Model.Root.Status);
        var anemic = new AnemicModel
        {
            Root = root,
            Variants = Model.Variants.ToList(),
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> AddVariant(IProductVariant variant)
    {
        var validator = new IsNotArchivedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Cannot add variant to an archived product.");

        var variants = Model.Variants.ToList();
        variants.Add(variant);
        var tempModel = new AnemicModel
        {
            Root = Model.Root,
            Variants = variants,
            Media = Model.Media.ToList()
        };

        var skuValidator = new UniqueSkuValidator();
        if (!skuValidator.IsSatisfiedBy(tempModel))
            return Fail("Variant SKU must be unique within the product.");

        var priceValidator = new VariantHasPriceValidator();
        if (!priceValidator.IsSatisfiedBy(variant))
            return Fail("Variant price must be greater than zero.");

        return Success(tempModel);
    }

    public AggregateResult<IProduct, IProductAnemicModel> RemoveVariant(string sku)
    {
        var validator = new IsNotArchivedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Cannot remove variant from an archived product.");

        var variants = Model.Variants.Where(v => v.Sku != sku).ToList();
        if (variants.Count == Model.Variants.Count)
            return Fail($"Variant with SKU '{sku}' not found.");

        var anemic = new AnemicModel
        {
            Root = Model.Root,
            Variants = variants,
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> UpdateVariantPrice(string sku, decimal newPrice)
    {
        var validator = new IsNotArchivedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Cannot update variant price on an archived product.");

        if (newPrice <= 0)
            return Fail("Price must be greater than zero.");

        var variants = Model.Variants.ToList();
        var existing = variants.FirstOrDefault(v => v.Sku == sku);
        if (existing is null)
            return Fail($"Variant with SKU '{sku}' not found.");

        var updated = ProductVariant.CreateInstance(existing.Sku, existing.Size, existing.Color, newPrice);
        var updatedVariants = variants.Where(v => v.Sku != sku).Append(updated).ToList();

        var anemic = new AnemicModel
        {
            Root = Model.Root,
            Variants = updatedVariants,
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> PublishProduct()
    {
        var draftValidator = new IsDraftValidator();
        if (!draftValidator.IsSatisfiedBy(Model))
            return Fail("Only draft products can be published.");

        var variantValidator = new HasAtLeastOneVariantValidator();
        if (!variantValidator.IsSatisfiedBy(Model))
            return Fail("Product must have at least one variant to be published.");

        var imageValidator = new HasImageForPublishValidator();
        if (!imageValidator.IsSatisfiedBy(Model))
            return Fail("Product must have at least one image to be published.");

        var root = ProductRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.Description,
            Model.Root.CategoryId,
            "Published");

        var anemic = new AnemicModel
        {
            Root = root,
            Variants = Model.Variants.ToList(),
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> ArchiveProduct()
    {
        var publishedValidator = new IsPublishedValidator();
        if (!publishedValidator.IsSatisfiedBy(Model))
            return Fail("Only published products can be archived.");

        var root = ProductRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.Description,
            Model.Root.CategoryId,
            "Archived");

        var anemic = new AnemicModel
        {
            Root = root,
            Variants = Model.Variants.ToList(),
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }

    public AggregateResult<IProduct, IProductAnemicModel> RestoreProduct()
    {
        var archivedValidator = new IsArchivedValidator();
        if (!archivedValidator.IsSatisfiedBy(Model))
            return Fail("Only archived products can be restored.");

        var root = ProductRoot.CreateInstance(
            Model.Root.Name,
            Model.Root.Description,
            Model.Root.CategoryId,
            "Draft");

        var anemic = new AnemicModel
        {
            Root = root,
            Variants = Model.Variants.ToList(),
            Media = Model.Media.ToList()
        };

        return Success(anemic);
    }
}
