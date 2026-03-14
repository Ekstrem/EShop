namespace Payment.Application.Queries;

using MediatR;
using Payment.InternalContracts;

public sealed class GetPaymentStatusQuery : IRequest<PaymentReadModel?>
{
    public Guid PaymentId { get; init; }
}
