namespace Payment.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class RequestRefundCommand : IRequest<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public Guid PaymentId { get; init; }
    public decimal RefundAmount { get; init; }
}
