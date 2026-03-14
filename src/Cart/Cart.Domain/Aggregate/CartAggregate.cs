namespace Cart.Domain.Aggregate;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain.Abstraction;
using Cart.Domain.Implementation;

public static class CartAggregate
{
    public static AggregateResult<ICart, ICartAnemicModel> AddItemToCart(
        ICartAnemicModel model,
        Guid variantId,
        string productName,
        string sku,
        int quantity,
        decimal unitPrice)
    {
        var items = model.Items.ToList();
        var existing = items.FirstOrDefault(i => i.VariantId == variantId);

        if (existing != null)
        {
            items.Remove(existing);
            var updated = CartItem.CreateInstance(
                variantId, productName, sku,
                existing.Quantity + quantity, unitPrice);
            items.Add(updated);
        }
        else
        {
            items.Add(CartItem.CreateInstance(variantId, productName, sku, quantity, unitPrice));
        }

        var newModel = new AnemicModel
        {
            Root = model.Root,
            Items = items.AsReadOnly(),
            AppliedPromoCode = model.AppliedPromoCode,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "ItemAddedToCart");
    }

    public static AggregateResult<ICart, ICartAnemicModel> RemoveItemFromCart(
        ICartAnemicModel model,
        Guid variantId)
    {
        var items = model.Items.Where(i => i.VariantId != variantId).ToList();

        var newModel = new AnemicModel
        {
            Root = model.Root,
            Items = items.AsReadOnly(),
            AppliedPromoCode = model.AppliedPromoCode,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "ItemRemovedFromCart");
    }

    public static AggregateResult<ICart, ICartAnemicModel> UpdateCartItemQuantity(
        ICartAnemicModel model,
        Guid variantId,
        int newQuantity)
    {
        var items = model.Items.ToList();
        var existing = items.FirstOrDefault(i => i.VariantId == variantId);

        if (existing != null)
        {
            items.Remove(existing);
            var updated = CartItem.CreateInstance(
                existing.VariantId, existing.ProductName, existing.Sku,
                newQuantity, existing.UnitPrice);
            items.Add(updated);
        }

        var newModel = new AnemicModel
        {
            Root = model.Root,
            Items = items.AsReadOnly(),
            AppliedPromoCode = model.AppliedPromoCode,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "CartItemQuantityUpdated");
    }

    public static AggregateResult<ICart, ICartAnemicModel> ApplyPromoCode(
        ICartAnemicModel model,
        string code,
        decimal discountPercent,
        decimal discountAmount)
    {
        var promo = PromoCode.CreateInstance(code, discountPercent, discountAmount);

        var newModel = new AnemicModel
        {
            Root = model.Root,
            Items = model.Items,
            AppliedPromoCode = promo,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "PromoCodeApplied");
    }

    public static AggregateResult<ICart, ICartAnemicModel> RemovePromoCode(
        ICartAnemicModel model)
    {
        var newModel = new AnemicModel
        {
            Root = model.Root,
            Items = model.Items,
            AppliedPromoCode = null,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "PromoCodeRemoved");
    }

    public static AggregateResult<ICart, ICartAnemicModel> MergeCarts(
        ICartAnemicModel target,
        ICartAnemicModel source)
    {
        var mergedItems = target.Items.ToList();

        foreach (var sourceItem in source.Items)
        {
            var existing = mergedItems.FirstOrDefault(i => i.VariantId == sourceItem.VariantId);
            if (existing != null)
            {
                mergedItems.Remove(existing);
                var merged = CartItem.CreateInstance(
                    existing.VariantId, existing.ProductName, existing.Sku,
                    existing.Quantity + sourceItem.Quantity, existing.UnitPrice);
                mergedItems.Add(merged);
            }
            else
            {
                mergedItems.Add(sourceItem);
            }
        }

        var newModel = new AnemicModel
        {
            Root = target.Root,
            Items = mergedItems.AsReadOnly(),
            AppliedPromoCode = target.AppliedPromoCode ?? source.AppliedPromoCode,
            ShippingAddress = target.ShippingAddress ?? source.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "CartsMerged");
    }

    public static AggregateResult<ICart, ICartAnemicModel> PlaceOrder(
        ICartAnemicModel model)
    {
        var newRoot = CartRoot.CreateInstance(
            model.Root.CustomerId,
            model.Root.SessionId,
            "CheckedOut",
            model.Root.CreatedAt);

        var newModel = new AnemicModel
        {
            Root = newRoot,
            Items = model.Items,
            AppliedPromoCode = model.AppliedPromoCode,
            ShippingAddress = model.ShippingAddress
        };

        return AggregateResult<ICart, ICartAnemicModel>.Create(newModel, "OrderPlaced");
    }
}
