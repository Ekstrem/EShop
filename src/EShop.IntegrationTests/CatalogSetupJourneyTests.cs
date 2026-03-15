namespace EShop.IntegrationTests;

using Xunit;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;

using Category.Domain.Abstraction;
using Category.Domain.Implementation;
using Product.Domain.Abstraction;
using Product.Domain.Implementation;
using StockItem.Domain.Abstraction;
using StockItem.Domain.Implementation;

using CategoryAggregate = Category.Domain.Implementation.Aggregate;
using CategoryAnemicModel = Category.Domain.Implementation.AnemicModel;
using ProductAggregate = Product.Domain.Implementation.Aggregate;
using ProductAnemicModel = Product.Domain.Implementation.AnemicModel;
using StockAnemicModel = StockItem.Domain.Implementation.AnemicModel;
using StockAggregate = StockItem.Domain.Implementation.Aggregate;

/// <summary>
/// Integration tests verifying the catalog setup journey across
/// Category, Product, and StockItem bounded contexts.
/// Journey: Create category hierarchy -> Create product -> Publish product -> Initialize stock
/// </summary>
public class CatalogSetupJourneyTests
{
    [Fact]
    public void FullCatalogSetup_CreateParentAndChildCategory_ThenProductWithVariants_ThenPublish_ThenReplenishStock()
    {
        // Step 1: Create parent category "Electronics"
        var parentCatAggregate = CategoryAggregate.CreateInstance(new CategoryAnemicModel());
        var parentCatResult = parentCatAggregate.CreateCategory(
            name: "Electronics",
            parentId: null,
            depth: 0,
            sortOrder: 1,
            siblingNames: new List<string>());

        Assert.True(parentCatResult.IsSuccess);
        Assert.Equal("Electronics", parentCatResult.Model!.Root.Name);
        Assert.Null(parentCatResult.Model.Root.ParentId);
        Assert.Equal(0, parentCatResult.Model.Root.Depth);
        Assert.Equal("Active", parentCatResult.Model.Root.Status);

        // Simulate persistence assigning an ID
        var parentCategoryId = Guid.NewGuid();

        // Step 2: Create child category "Laptop Accessories" under Electronics
        var childCatAggregate = CategoryAggregate.CreateInstance(new CategoryAnemicModel());
        var childCatResult = childCatAggregate.CreateCategory(
            name: "Laptop Accessories",
            parentId: parentCategoryId,
            depth: 1,
            sortOrder: 1,
            siblingNames: new List<string>());

        Assert.True(childCatResult.IsSuccess);
        Assert.Equal("Laptop Accessories", childCatResult.Model!.Root.Name);
        Assert.Equal(parentCategoryId, childCatResult.Model.Root.ParentId);
        Assert.Equal(1, childCatResult.Model.Root.Depth);
        Assert.Equal("Active", childCatResult.Model.Root.Status);

        var childCategoryId = Guid.NewGuid();

        // Step 3: Create product in child category with variants and media
        var prodAggregate = ProductAggregate.CreateInstance(new ProductAnemicModel());
        var variants = new List<IProductVariant>
        {
            ProductVariant.CreateInstance("LPSTD-BLK-001", "Standard", "Black", 49.99m),
            ProductVariant.CreateInstance("LPSTD-SLV-002", "Standard", "Silver", 54.99m)
        };
        var media = new List<IProductMedia>
        {
            ProductMedia.CreateInstance("https://cdn.example.com/laptop-stand-1.jpg", "Laptop stand front view", 1),
            ProductMedia.CreateInstance("https://cdn.example.com/laptop-stand-2.jpg", "Laptop stand side view", 2)
        };

        var prodResult = prodAggregate.CreateProduct(
            name: "Ergonomic Laptop Stand",
            description: "Adjustable aluminum laptop stand for improved ergonomics",
            categoryId: childCategoryId,
            variants: variants,
            media: media);

        Assert.True(prodResult.IsSuccess);
        Assert.Equal("Ergonomic Laptop Stand", prodResult.Model!.Root.Name);
        Assert.Equal(childCategoryId, prodResult.Model.Root.CategoryId);
        Assert.Equal("Draft", prodResult.Model.Root.Status);
        Assert.Equal(2, prodResult.Model.Variants.Count);
        Assert.Equal(2, prodResult.Model.Media.Count);

        // Step 4: Publish the product (requires at least one variant and one image)
        var publishAggregate = ProductAggregate.CreateInstance(prodResult.Model);
        var publishResult = publishAggregate.PublishProduct();

        Assert.True(publishResult.IsSuccess);
        Assert.Equal("Published", publishResult.Model!.Root.Status);
        Assert.Equal("Ergonomic Laptop Stand", publishResult.Model.Root.Name);
        Assert.Equal(2, publishResult.Model.Variants.Count);

        // Step 5: Initialize stock for the first variant (Black) in a warehouse
        var variantId = Guid.NewGuid(); // simulates the persisted variant ID
        var warehouseId = Guid.NewGuid();

        var initialStockModel = new StockAnemicModel
        {
            Root = StockItemRoot.CreateInstance(
                variantId: variantId,
                warehouseId: warehouseId,
                total: 0,
                reserved: 0,
                lowStockThreshold: 10),
            Reservations = new List<IReservation>()
        };

        var stockAggregate = StockAggregate.CreateInstance(initialStockModel);

        // Replenish stock with initial inventory
        var replenishResult = stockAggregate.ReplenishStock(100);

        Assert.True(replenishResult.IsSuccess);
        Assert.Equal(100, replenishResult.Model!.Root.Total);
        Assert.Equal(0, replenishResult.Model.Root.Reserved);
        Assert.Equal("InStock", replenishResult.Model.Root.Status);

        // Step 6: Replenish additional stock (second batch)
        var stockAggregate2 = StockAggregate.CreateInstance(replenishResult.Model);
        var secondReplenish = stockAggregate2.ReplenishStock(50);

        Assert.True(secondReplenish.IsSuccess);
        Assert.Equal(150, secondReplenish.Model!.Root.Total);
        Assert.Equal("InStock", secondReplenish.Model.Root.Status);
    }

    [Fact]
    public void CatalogSetup_CannotPublishProductWithoutImage()
    {
        // Create product with a variant but no media
        var prodAggregate = ProductAggregate.CreateInstance(new ProductAnemicModel());
        var variants = new List<IProductVariant>
        {
            ProductVariant.CreateInstance("SKU-001", "Large", "Red", 29.99m)
        };
        var media = new List<IProductMedia>(); // empty

        var prodResult = prodAggregate.CreateProduct(
            name: "Test Product",
            description: "A product without images",
            categoryId: Guid.NewGuid(),
            variants: variants,
            media: media);

        Assert.True(prodResult.IsSuccess);
        Assert.Equal("Draft", prodResult.Model!.Root.Status);

        // Attempt to publish - should fail because no image
        var publishAggregate = ProductAggregate.CreateInstance(prodResult.Model);
        var publishResult = publishAggregate.PublishProduct();

        Assert.False(publishResult.IsSuccess);
        Assert.Contains("image", publishResult.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CatalogSetup_CategoryDepthExceeded_Fails()
    {
        // Attempt to create a category at depth 5 (exceeds max depth of 4)
        var catAggregate = CategoryAggregate.CreateInstance(new CategoryAnemicModel());
        var result = catAggregate.CreateCategory(
            name: "Too Deep Category",
            parentId: Guid.NewGuid(),
            depth: 5,
            sortOrder: 1,
            siblingNames: new List<string>());

        Assert.False(result.IsSuccess);
        Assert.Contains("depth", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CatalogSetup_DuplicateCategoryNameAmongSiblings_Fails()
    {
        var catAggregate = CategoryAggregate.CreateInstance(new CategoryAnemicModel());
        var result = catAggregate.CreateCategory(
            name: "Electronics",
            parentId: null,
            depth: 0,
            sortOrder: 2,
            siblingNames: new List<string> { "Electronics", "Books" });

        Assert.False(result.IsSuccess);
        Assert.Contains("unique", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CatalogSetup_ReplenishZeroStock_Fails()
    {
        var variantId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();

        var stockModel = new StockAnemicModel
        {
            Root = StockItemRoot.CreateInstance(variantId, warehouseId, 50, 0, 5),
            Reservations = new List<IReservation>()
        };

        var stockAggregate = StockAggregate.CreateInstance(stockModel);
        var result = stockAggregate.ReplenishStock(0);

        Assert.False(result.IsSuccess);
        Assert.Contains("greater than zero", result.ErrorMessage!, StringComparison.OrdinalIgnoreCase);
    }
}
