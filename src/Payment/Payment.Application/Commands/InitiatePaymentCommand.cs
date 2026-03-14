namespace Payment.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class InitiatePaymentCommand : IRequest<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public Guid OrderId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
}
