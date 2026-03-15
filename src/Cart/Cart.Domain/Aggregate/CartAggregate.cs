namespace Cart.Domain.Aggregate;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Cart.Domain.Abstraction;
using Cart.Domain.Implementation;
using EShop.Contracts;

public static class CartAggregate
{
    private static AggregateResult<ICart, ICartAnemicModel> Success(
        ICartAnemicModel oldModel, ICartAnemicModel newModel)
    {
        var data = BusinessOperationData<ICart, ICartAnemicModel>
            .Commit<ICart, ICartAnemicModel>(oldModel, newModel);
        return new AggregateResultSuccess<ICart, ICartAnemicModel>(data);
    }

    private static AggregateResult<ICart, ICartAnemicModel> Fail(
        ICartAnemicModel model, string error)
    {
        var data = BusinessOperationData<ICart, ICartAnemicModel>
            .Commit<ICart, ICartAnemicModel>(model, model);
        return new AggregateResultException<ICart, ICartAnemicModel>(
            data, new FailedSpecification<ICart, ICartAnemicModel>(error));
    }

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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(target, newModel);
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

        return Success(model, newModel);
    }
}
