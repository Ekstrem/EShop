namespace Cart.Application.Queries;

using Cart.Domain.Abstraction;
using MediatR;

public class GetCartQuery : IRequest<ICartAnemicModel?>
{
    public Guid CartId { get; set; }
}
