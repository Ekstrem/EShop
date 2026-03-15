namespace Order.Domain.Aggregate;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Order.Domain.Abstraction;
using Order.Domain.Implementation;
using EShop.Contracts;

public static class OrderAggregate
{
    private static AggregateResult<IOrder, IOrderAnemicModel> Success(
        IOrderAnemicModel oldModel, IOrderAnemicModel newModel)
    {
        var data = BusinessOperationData<IOrder, IOrderAnemicModel>
            .Commit<IOrder, IOrderAnemicModel>(oldModel, newModel);
        return new AggregateResultSuccess<IOrder, IOrderAnemicModel>(data);
    }

    private static AggregateResult<IOrder, IOrderAnemicModel> Fail(
        IOrderAnemicModel model, string error)
    {
        var data = BusinessOperationData<IOrder, IOrderAnemicModel>
            .Commit<IOrder, IOrderAnemicModel>(model, model);
        return new AggregateResultException<IOrder, IOrderAnemicModel>(
            data, new FailedSpecification<IOrder, IOrderAnemicModel>(error));
    }

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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(model, newModel);
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

        return Success(model, newModel);
    }
}
