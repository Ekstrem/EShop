namespace Payment.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using MediatR;
using Payment.Domain;
using Payment.Domain.Abstraction;

public sealed class VoidPaymentCommand : IRequest<AggregateResult<IPayment, IPaymentAnemicModel>>
{
    public Guid PaymentId { get; init; }
}
