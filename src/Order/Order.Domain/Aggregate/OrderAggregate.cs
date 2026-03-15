namespace Order.Domain.Aggregate;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Order.Domain.Abstraction;
using Order.Domain.Implementation;

public static class OrderAggregate
{
    public static AggregateResult<IOrder, IOrderAnemicModel> ConfirmPayment(
        IOrderAnemicModel model)
    {
        var newRoot = OrderRoot.CreateInstance(
            model.Root.CustomerId,
            model.Root.OrderNumber,
            "Paid",
            model.Root.CreatedAt,
            model.Root.ShippingAddress);

        var newModel = new AnemicModel
        {
            Root = newRoot,
            Lines = model.Lines,
            OrderTotal = model.OrderTotal
        };

        return AggregateResult<IOrder, IOrderAnemicModel>.Create(newModel, "PaymentConfirmed");
    }

    public static AggregateResult<IOrder, IOrderAnemicModel> CancelOrder(
        IOrderAnemicModel model)
    {
        var newRoot = OrderRoot.CreateInstance(
            model.Root.CustomerId,
            model.Root.OrderNumber,
            "Cancelled",
            model.Root.CreatedAt,
            model.Root.ShippingAddress);

        var newModel = new AnemicModel
        {
            Root = newRoot,
            Lines = model.Lines,
            OrderTotal = model.OrderTotal
        };

        return AggregateResult<IOrder, IOrderAnemicModel>.Create(newModel, "OrderCancelled");
    }

    public static AggregateResult<IOrder, IOrderAnemicModel> ConfirmShipment(
        IOrderAnemicModel model)
    {
        var newRoot = OrderRoot.CreateInstance(
            model.Root.CustomerId,
            model.Root.OrderNumber,
            "Shipped",
            model.Root.CreatedAt,
            model.Root.ShippingAddress);

        var newModel = new AnemicModel
        {
            Root = newRoot,
            Lines = model.Lines,
            OrderTotal = model.OrderTotal
        };

        return AggregateResult<IOrder, IOrderAnemicModel>.Create(newModel, "ShipmentConfirmed");
    }

    public static AggregateResult<IOrder, IOrderAnemicModel> ConfirmDelivery(
        IOrderAnemicModel model)
    {
        var newRoot = OrderRoot.CreateInstance(
            model.Root.CustomerId,
            model.Root.OrderNumber,
            "Delivered",
            model.Root.CreatedAt,
            model.Root.ShippingAddress);

        var newModel = new AnemicModel
        {
            Root = newRoot,
            Lines = model.Lines,
            OrderTotal = model.OrderTotal
        };

        return AggregateResult<IOrder, IOrderAnemicModel>.Create(newModel, "DeliveryConfirmed");
    }
}
