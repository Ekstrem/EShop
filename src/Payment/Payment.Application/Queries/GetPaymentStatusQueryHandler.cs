namespace Payment.Application.Queries;

using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.InternalContracts;

public sealed class GetPaymentStatusQueryHandler
    : IRequestHandler<GetPaymentStatusQuery, PaymentReadModel?>
{
    private readonly IQueryRepository<IPayment, IPaymentAnemicModel> _repository;

    public GetPaymentStatusQueryHandler(
        IQueryRepository<IPayment, IPaymentAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<PaymentReadModel?> Handle(
        GetPaymentStatusQuery request, CancellationToken cancellationToken)
    {
        var model = await _repository.GetByIdAsync(request.PaymentId, cancellationToken);
        if (model is null)
            return null;

        return new PaymentReadModel
        {
            Id = request.PaymentId,
            OrderId = model.Root.OrderId,
            Amount = model.Root.Amount,
            Currency = model.Root.Currency,
            PaymentMethod = model.Root.PaymentMethod,
            Status = model.Root.Status,
            TotalRefunded = model.TotalRefunded,
            Transactions = model.Transactions.Select(t => new TransactionReadModel
            {
                ProviderTransactionId = t.ProviderTransactionId,
                Type = t.Type,
                Amount = t.Amount,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }).ToList()
        };
    }
}
