namespace Payment.Application.Commands;

using Hive.SeedWorks.Result;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class VoidPaymentCommand : IRequest<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public Guid PaymentId { get; init; }
}
