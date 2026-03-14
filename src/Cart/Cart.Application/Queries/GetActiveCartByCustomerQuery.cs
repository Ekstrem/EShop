namespace Cart.Application.Queries;

using Cart.Domain.Abstraction;
using MediatR;

public class GetActiveCartByCustomerQuery : IRequest<ICartAnemicModel?>
{
    public Guid CustomerId { get; set; }
}
