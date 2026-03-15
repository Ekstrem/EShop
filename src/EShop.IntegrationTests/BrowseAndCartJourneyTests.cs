namespace EShop.IntegrationTests;

using Xunit;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;

using Product.Domain.Abstraction;
using Product.Domain.Implementation;
using Cart.Domain.Abstraction;
using Cart.Domain.Implementation;
using Cart.Domain.Aggregate;
using DiscountCode.Domain.Abstraction;
using DiscountCode.Domain.Implementation;

using ProductAggregate = Product.Domain.Implementation.Aggregate;
using ProductAnemicModel = Product.Domain.Implementation.AnemicModel;
using CartAnemicModel = Cart.Domain.Implementation.AnemicModel;
using DiscountAnemicModel = DiscountCode.Domain.Implementation.AnemicModel;
using DiscountAggregate = DiscountCode.Domain.Implementation.Aggregate;

/// <summary>
/// Integration tests verifying the browse-and-cart journey across
/// Product, Cart, and DiscountCode bounded contexts.
/// Journey: Create/publish product -> Add to cart -> Apply discount -> Update quantity -> Place order
/// </summary>
public class BrowseAndCartJourneyTests
{
    /// <summary>
    /// Creates a published product with one variant and one image for use in cart tests.
    /// </summary>
    private static IProductAnemicModel CreatePublishedProduct(
        string name,
        string sku,
        decimal price,
        Guid categoryId)
    {
        var prodAggregate = ProductAggregate.CreateInstance(new ProductAnemicModel());
        var variants = new List<IProductVariant>
        {
            ProductVariant.CreateInstance(sku, "Standard", "Black", price)
        };
        var media = new List<IProductMedia>
        {
            ProductMedia.CreateInstance("https://cdn.example.com/product.jpg", "Product image", 1)
        };

        var createResult = prodAggregate.CreateProduct(name, "Test product", categoryId, variants, media);
        var publishResult = ProductAggregate.CreateInstance(createResult.Model()!).PublishProduct();
        return publishResult.Model()!;
    }

    /// <summary>
    /// Creates a fresh active cart for a given customer and session.
    /// </summary>
    private static ICartAnemicModel CreateEmptyCart(Guid customerId, Guid sessionId)
    {
        return new CartAnemicModel
        {
            Root = CartRoot.CreateInstance(customerId, sessionId),
            Items = new List<ICartItem>().AsReadOnly(),
            AppliedPromoCode = null,
            ShippingAddress = null
        };
    }

    [Fact]
    public void FullBrowseAndCart_AddItem_ApplyDiscount_UpdateQuantity_PlaceOrder()
    {
        var categoryId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var variantId = Guid.NewGuid(); // simulates persisted variant ID

        // Step 1: Create and publish a product
        var product = CreatePublishedProduct("Wireless Keyboard", "KB-WL-001", 79.99m, categoryId);
        Assert.Equal("Published", product.Root.Status);
        Assert.Single(product.Variants);

        // Step 2: Create an empty cart and add the product item
        var cart = CreateEmptyCart(customerId, sessionId);
        var addResult = CartAggregate.AddItemToCart(
            model: cart,
            variantId: variantId,
            productName: product.Root.Name,
            sku: product.Variants[0].Sku,
            quantity: 1,
            unitPrice: product.Variants[0].Price);

        Assert.True(addResult.IsSuccess());
        Assert.Single(addResult.Model()!.Items);
        Assert.Equal("KB-WL-001", addResult.Model().Items[0].Sku);
        Assert.Equal(1, addResult.Model().Items[0].Quantity);
        Assert.Equal(79.99m, addResult.Model().Items[0].UnitPrice);
        Assert.Equal("Active", addResult.Model().Root.Status);

        // Step 3: Generate and validate a discount code in the DiscountCode context
        var discountAggregate = DiscountAggregate.CreateInstance(new DiscountAnemicModel());
        var generateResult = discountAggregate.GenerateDiscountCode(
            code: "SAVE10NOW",
            promotionId: null,
            maxUsage: 100,
            expiresAt: DateTime.UtcNow.AddDays(30));

        Assert.True(generateResult.IsSuccess());
        Assert.Equal("SAVE10NOW", generateResult.Model()!.Root.Code);
        Assert.Equal("Active", generateResult.Model().Root.Status);

        // Validate the discount code before applying
        var validateAggregate = DiscountAggregate.CreateInstance(generateResult.Model());
        var validateResult = validateAggregate.ValidateDiscountCode();
        Assert.True(validateResult.IsSuccess());

        // Step 4: Apply the discount code to the cart
        // Discount of 10% translates to $7.999 off the $79.99 item
        var applyResult = CartAggregate.ApplyPromoCode(
            model: addResult.Model(),
            code: "SAVE10NOW",
            discountPercent: 10m,
            discountAmount: 8.00m);

        Assert.True(applyResult.IsSuccess());
        Assert.NotNull(applyResult.Model()!.AppliedPromoCode);
        Assert.Equal("SAVE10NOW", applyResult.Model().AppliedPromoCode!.Code);
        Assert.Equal(10m, applyResult.Model().AppliedPromoCode.DiscountPercent);
        Assert.Equal(8.00m, applyResult.Model().AppliedPromoCode.DiscountAmount);

        // Step 5: Update cart item quantity (buy 3 keyboards)
        var updateResult = CartAggregate.UpdateCartItemQuantity(
            model: applyResult.Model(),
            variantId: variantId,
            newQuantity: 3);

        Assert.True(updateResult.IsSuccess());
        Assert.Single(updateResult.Model()!.Items);
        Assert.Equal(3, updateResult.Model().Items[0].Quantity);
        // Promo code should still be applied after quantity update
        Assert.NotNull(updateResult.Model().AppliedPromoCode);
        Assert.Equal("SAVE10NOW", updateResult.Model().AppliedPromoCode!.Code);

        // Step 6: Place the order (transitions cart to CheckedOut)
        var placeOrderResult = CartAggregate.PlaceOrder(updateResult.Model());

        Assert.True(placeOrderResult.IsSuccess());
        Assert.Equal("CheckedOut", placeOrderResult.Model()!.Root.Status);
        Assert.Equal(3, placeOrderResult.Model().Items[0].Quantity);
        Assert.NotNull(placeOrderResult.Model().AppliedPromoCode);

        // Step 7: Redeem the discount code (marks usage in DiscountCode context)
        var orderId = Guid.NewGuid();
        var redeemAggregate = DiscountAggregate.CreateInstance(generateResult.Model());
        var redeemResult = redeemAggregate.RedeemDiscountCode(orderId);

        Assert.True(redeemResult.IsSuccess());
        Assert.Equal(1, redeemResult.Model()!.Root.UsageCount);
        Assert.Single(redeemResult.Model().Redemptions);
        Assert.Equal(orderId, redeemResult.Model().Redemptions[0].OrderId);
    }

    [Fact]
    public void BrowseAndCart_AddMultipleProductsToCart()
    {
        var categoryId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();

        // Create two published products
        var keyboard = CreatePublishedProduct("Wireless Keyboard", "KB-001", 79.99m, categoryId);
        var mouse = CreatePublishedProduct("Wireless Mouse", "MS-001", 39.99m, categoryId);

        var keyboardVariantId = Guid.NewGuid();
        var mouseVariantId = Guid.NewGuid();

        // Add keyboard to cart
        var cart = CreateEmptyCart(customerId, sessionId);
        var addKeyboard = CartAggregate.AddItemToCart(
            cart, keyboardVariantId, keyboard.Root.Name,
            keyboard.Variants[0].Sku, 1, keyboard.Variants[0].Price);

        Assert.True(addKeyboard.IsSuccess());
        Assert.Single(addKeyboard.Model()!.Items);

        // Add mouse to cart
        var addMouse = CartAggregate.AddItemToCart(
            addKeyboard.Model(), mouseVariantId, mouse.Root.Name,
            mouse.Variants[0].Sku, 2, mouse.Variants[0].Price);

        Assert.True(addMouse.IsSuccess());
        Assert.Equal(2, addMouse.Model()!.Items.Count);

        // Verify both items are in the cart
        var kbItem = addMouse.Model().Items.First(i => i.VariantId == keyboardVariantId);
        var msItem = addMouse.Model().Items.First(i => i.VariantId == mouseVariantId);
        Assert.Equal(1, kbItem.Quantity);
        Assert.Equal(2, msItem.Quantity);
    }

    [Fact]
    public void BrowseAndCart_AddSameItemTwice_MergesQuantity()
    {
        var categoryId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var product = CreatePublishedProduct("USB Hub", "HUB-001", 24.99m, categoryId);

        var cart = CreateEmptyCart(customerId, sessionId);

        // Add 2 units
        var add1 = CartAggregate.AddItemToCart(
            cart, variantId, product.Root.Name,
            product.Variants[0].Sku, 2, product.Variants[0].Price);

        // Add 3 more units of the same variant
        var add2 = CartAggregate.AddItemToCart(
            add1.Model()!, variantId, product.Root.Name,
            product.Variants[0].Sku, 3, product.Variants[0].Price);

        Assert.True(add2.IsSuccess());
        Assert.Single(add2.Model()!.Items);
        Assert.Equal(5, add2.Model().Items[0].Quantity); // 2 + 3 = 5
    }

    [Fact]
    public void BrowseAndCart_RemovePromoCode_ThenPlaceOrder()
    {
        var customerId = Guid.NewGuid();
        var sessionId = Guid.NewGuid();
        var variantId = Guid.NewGuid();

        var cart = CreateEmptyCart(customerId, sessionId);

        // Add item
        var addResult = CartAggregate.AddItemToCart(
            cart, variantId, "Test Product", "TP-001", 1, 50.00m);

        // Apply promo
        var applyResult = CartAggregate.ApplyPromoCode(
            addResult.Model()!, "PROMO20", 20m, 10.00m);
        Assert.NotNull(applyResult.Model()!.AppliedPromoCode);

        // Remove promo
        var removeResult = CartAggregate.RemovePromoCode(applyResult.Model());
        Assert.True(removeResult.IsSuccess());
        Assert.Null(removeResult.Model()!.AppliedPromoCode);

        // Place order without promo
        var orderResult = CartAggregate.PlaceOrder(removeResult.Model());
        Assert.True(orderResult.IsSuccess());
        Assert.Equal("CheckedOut", orderResult.Model()!.Root.Status);
        Assert.Null(orderResult.Model().AppliedPromoCode);
    }

    [Fact]
    public void BrowseAndCart_DiscountCodeExhausted_FailsValidation()
    {
        // Generate a discount code with max usage of 1
        var discountAggregate = DiscountAggregate.CreateInstance(new DiscountAnemicModel());
        var generateResult = discountAggregate.GenerateDiscountCode(
            code: "ONETIME",
            promotionId: null,
            maxUsage: 1,
            expiresAt: DateTime.UtcNow.AddDays(7));

        Assert.True(generateResult.IsSuccess());

        // Redeem it once
        var redeemAggregate = DiscountAggregate.CreateInstance(generateResult.Model()!);
        var redeemResult = redeemAggregate.RedeemDiscountCode(Guid.NewGuid());
        Assert.True(redeemResult.IsSuccess());
        Assert.Equal("Exhausted", redeemResult.Model()!.Root.Status);

        // Try to validate the exhausted code - should fail
        var validateAggregate = DiscountAggregate.CreateInstance(redeemResult.Model());
        var validateResult = validateAggregate.ValidateDiscountCode();
        Assert.False(validateResult.IsSuccess());
    }

    [Fact]
    public void BrowseAndCart_MergeCarts_CombinesItems()
    {
        var customerId = Guid.NewGuid();
        var sessionId1 = Guid.NewGuid();
        var sessionId2 = Guid.NewGuid();
        var variantA = Guid.NewGuid();
        var variantB = Guid.NewGuid();

        // Cart 1: anonymous browsing session with one item
        var cart1 = CreateEmptyCart(customerId, sessionId1);
        var addA = CartAggregate.AddItemToCart(
            cart1, variantA, "Product A", "SKU-A", 2, 30.00m);

        // Cart 2: logged-in session with a different item
        var cart2 = CreateEmptyCart(customerId, sessionId2);
        var addB = CartAggregate.AddItemToCart(
            cart2, variantB, "Product B", "SKU-B", 1, 50.00m);

        // Merge cart2 into cart1
        var mergeResult = CartAggregate.MergeCarts(addA.Model()!, addB.Model()!);

        Assert.True(mergeResult.IsSuccess());
        Assert.Equal(2, mergeResult.Model()!.Items.Count);
        Assert.Contains(mergeResult.Model().Items, i => i.VariantId == variantA && i.Quantity == 2);
        Assert.Contains(mergeResult.Model().Items, i => i.VariantId == variantB && i.Quantity == 1);
    }
}
