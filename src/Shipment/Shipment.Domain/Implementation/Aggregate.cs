namespace Shipment.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Shipment.Domain.Abstraction;
using Shipment.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IShipmentAnemicModel Model { get; }

    private Aggregate(IShipmentAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IShipmentAnemicModel model) => new(model);

    private AggregateResult<IShipment, IShipmentAnemicModel> Success(IShipmentAnemicModel newModel)
    {
        var data = BusinessOperationData<IShipment, IShipmentAnemicModel>
            .Commit<IShipment, IShipmentAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IShipment, IShipmentAnemicModel>(data);
    }

    private AggregateResult<IShipment, IShipmentAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IShipment, IShipmentAnemicModel>
            .Commit<IShipment, IShipmentAnemicModel>(Model, Model);
        return new AggregateResultException<IShipment, IShipmentAnemicModel>(
            data, new FailedSpecification<IShipment, IShipmentAnemicModel>(error));
    }

    public AggregateResult<IShipment, IShipmentAnemicModel> CreateShipment(
        Guid orderId,
        string carrier,
        string shippingAddress,
        IReadOnlyList<IShipmentItem> items)
    {
        if (items.Count < 1)
            return Fail(new HasItemsValidator().Reason);

        var root = ShipmentRoot.CreateInstance(
            id: Guid.NewGuid(),
            orderId: orderId,
            trackingNumber: string.Empty,
            carrier: carrier,
            shippingAddress: shippingAddress,
            status: "Pending",
            createdAt: DateTime.UtcNow);

        var anemic = new AnemicModel { Root = root, Items = items.ToList() };
        return Success(anemic);
    }

    public AggregateResult<IShipment, IShipmentAnemicModel> MarkAsPacked()
    {
        var isPendingValidator = new IsPendingValidator();
        if (!isPendingValidator.IsSatisfiedBy(Model))
            return Fail(isPendingValidator.Reason);

        var hasItemsValidator = new HasItemsValidator();
        if (!hasItemsValidator.IsSatisfiedBy(Model))
            return Fail(hasItemsValidator.Reason);

        var root = ShipmentRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            trackingNumber: Model.Root.TrackingNumber,
            carrier: Model.Root.Carrier,
            shippingAddress: Model.Root.ShippingAddress,
            status: "Packed",
            createdAt: Model.Root.CreatedAt);

        var anemic = new AnemicModel { Root = root, Items = Model.Items.ToList(), Label = Model.Label };
        return Success(anemic);
    }

    public AggregateResult<IShipment, IShipmentAnemicModel> DispatchShipment(
        string trackingNumber,
        string labelUrl)
    {
        var isPackedValidator = new IsPackedValidator();
        if (!isPackedValidator.IsSatisfiedBy(Model))
            return Fail(isPackedValidator.Reason);

        if (string.IsNullOrWhiteSpace(trackingNumber))
            return Fail(new HasTrackingNumberValidator().Reason);

        if (string.IsNullOrWhiteSpace(labelUrl))
            return Fail(new HasLabelValidator().Reason);

        var root = ShipmentRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            trackingNumber: trackingNumber,
            carrier: Model.Root.Carrier,
            shippingAddress: Model.Root.ShippingAddress,
            status: "Shipped",
            createdAt: Model.Root.CreatedAt);

        var label = ShippingLabel.CreateInstance(labelUrl, DateTime.UtcNow);
        var anemic = new AnemicModel { Root = root, Items = Model.Items.ToList(), Label = label };
        return Success(anemic);
    }

    public AggregateResult<IShipment, IShipmentAnemicModel> HandleCarrierUpdate(string newStatus)
    {
        var isNotDeliveredValidator = new IsNotDeliveredValidator();
        if (!isNotDeliveredValidator.IsSatisfiedBy(Model))
            return Fail(isNotDeliveredValidator.Reason);

        var sequentialValidator = new SequentialStatusValidator(newStatus);
        if (!sequentialValidator.IsSatisfiedBy(Model))
            return Fail(sequentialValidator.Reason);

        var root = ShipmentRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            trackingNumber: Model.Root.TrackingNumber,
            carrier: Model.Root.Carrier,
            shippingAddress: Model.Root.ShippingAddress,
            status: newStatus,
            createdAt: Model.Root.CreatedAt);

        var anemic = new AnemicModel { Root = root, Items = Model.Items.ToList(), Label = Model.Label };
        return Success(anemic);
    }
}
