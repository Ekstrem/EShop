namespace ReturnRequest.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using ReturnRequest.Domain.Abstraction;
using ReturnRequest.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IReturnRequestAnemicModel Model { get; }

    private Aggregate(IReturnRequestAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IReturnRequestAnemicModel model) => new(model);

    private AggregateResult<IReturnRequest, IReturnRequestAnemicModel> Success(IReturnRequestAnemicModel newModel)
    {
        var data = BusinessOperationData<IReturnRequest, IReturnRequestAnemicModel>
            .Commit<IReturnRequest, IReturnRequestAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IReturnRequest, IReturnRequestAnemicModel>(data);
    }

    private AggregateResult<IReturnRequest, IReturnRequestAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IReturnRequest, IReturnRequestAnemicModel>
            .Commit<IReturnRequest, IReturnRequestAnemicModel>(Model, Model);
        return new AggregateResultException<IReturnRequest, IReturnRequestAnemicModel>(
            data, new FailedSpecification<IReturnRequest, IReturnRequestAnemicModel>(error));
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> RequestReturn(
        Guid orderId,
        Guid customerId,
        string reason,
        IReadOnlyList<IReturnItem> items,
        DateTime orderDeliveredAt)
    {
        if (string.IsNullOrWhiteSpace(reason))
            return Fail(new HasReasonValidator().Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Guid.NewGuid(),
            orderId: orderId,
            customerId: customerId,
            rmaNumber: $"RMA-{Guid.NewGuid():N}"[..12],
            reason: reason,
            status: "Requested",
            requestedAt: DateTime.UtcNow);

        var anemic = new AnemicModel { Root = root, Items = items.ToList() };

        var withinReturnPeriodValidator = new WithinReturnPeriodValidator(orderDeliveredAt);
        if (!withinReturnPeriodValidator.IsSatisfiedBy(anemic))
            return Fail(withinReturnPeriodValidator.Reason);

        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> ApproveReturn(
        string labelUrl,
        string carrier)
    {
        var isRequestedValidator = new IsRequestedValidator();
        if (!isRequestedValidator.IsSatisfiedBy(Model))
            return Fail(isRequestedValidator.Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: Model.Root.Reason,
            status: "Approved",
            requestedAt: Model.Root.RequestedAt);

        var label = ReturnLabel.CreateInstance(labelUrl, carrier);
        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = label,
            RefundAmount = Model.RefundAmount
        };
        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> RejectReturn(string rejectionReason)
    {
        var isRequestedValidator = new IsRequestedValidator();
        if (!isRequestedValidator.IsSatisfiedBy(Model))
            return Fail(isRequestedValidator.Reason);

        if (string.IsNullOrWhiteSpace(rejectionReason))
            return Fail("Rejection reason is required.");

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: rejectionReason,
            status: "Rejected",
            requestedAt: Model.Root.RequestedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = Model.ReturnLabel,
            RefundAmount = Model.RefundAmount
        };
        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> ConfirmReturnShipped()
    {
        var isApprovedValidator = new IsApprovedValidator();
        if (!isApprovedValidator.IsSatisfiedBy(Model))
            return Fail(isApprovedValidator.Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: Model.Root.Reason,
            status: "ReturnShipped",
            requestedAt: Model.Root.RequestedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = Model.ReturnLabel,
            RefundAmount = Model.RefundAmount
        };
        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> ReceiveReturn()
    {
        var isReturnShippedValidator = new IsReturnShippedValidator();
        if (!isReturnShippedValidator.IsSatisfiedBy(Model))
            return Fail(isReturnShippedValidator.Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: Model.Root.Reason,
            status: "Received",
            requestedAt: Model.Root.RequestedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = Model.ReturnLabel,
            RefundAmount = Model.RefundAmount
        };
        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> CompleteReturn(decimal refundAmount)
    {
        var isReceivedValidator = new IsReceivedValidator();
        if (!isReceivedValidator.IsSatisfiedBy(Model))
            return Fail(isReceivedValidator.Reason);

        if (refundAmount <= 0)
            return Fail("Refund amount must be positive.");

        var totalItemsCost = Model.Items.Sum(i => i.Quantity * i.UnitPrice);
        if (refundAmount > totalItemsCost)
            return Fail(new RefundAmountValidator().Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: Model.Root.Reason,
            status: "Completed",
            requestedAt: Model.Root.RequestedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = Model.ReturnLabel,
            RefundAmount = refundAmount
        };
        return Success(anemic);
    }

    public AggregateResult<IReturnRequest, IReturnRequestAnemicModel> RejectReturnedItem(string rejectionReason)
    {
        var isReceivedValidator = new IsReceivedValidator();
        if (!isReceivedValidator.IsSatisfiedBy(Model))
            return Fail(isReceivedValidator.Reason);

        var root = ReturnRequestRoot.CreateInstance(
            id: Model.Root.Id,
            orderId: Model.Root.OrderId,
            customerId: Model.Root.CustomerId,
            rmaNumber: Model.Root.RmaNumber,
            reason: rejectionReason,
            status: "RejectedAfterInspection",
            requestedAt: Model.Root.RequestedAt);

        var anemic = new AnemicModel
        {
            Root = root,
            Items = Model.Items.ToList(),
            ReturnLabel = Model.ReturnLabel,
            RefundAmount = Model.RefundAmount
        };
        return Success(anemic);
    }
}
