namespace Payment.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class HandleWebhookCommand : IRequest<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public Guid PaymentId { get; init; }
    public string ProviderTransactionId { get; init; } = string.Empty;
    public string TransactionType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string TransactionStatus { get; init; } = string.Empty;
}
