namespace Payment.Application.Queries;

using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;
using Payment.InternalContracts;

public sealed class ListTransactionsQueryHandler
    : IRequestHandler<ListTransactionsQuery, IReadOnlyList<TransactionReadModel>>
{
    private readonly IQueryRepository<IPayment, IPaymentAnemicModel> _repository;

    public ListTransactionsQueryHandler(
        IQueryRepository<IPayment, IPaymentAnemicModel> repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TransactionReadModel>> Handle(
        ListTransactionsQuery request, CancellationToken cancellationToken)
    {
        var model = await _repository.GetByIdAsync(request.PaymentId, cancellationToken);
        if (model is null)
            return new List<TransactionReadModel>();

        return model.Transactions.Select(t => new TransactionReadModel
        {
            ProviderTransactionId = t.ProviderTransactionId,
            Type = t.Type,
            Amount = t.Amount,
            Status = t.Status,
            CreatedAt = t.CreatedAt
        }).ToList();
    }
}
