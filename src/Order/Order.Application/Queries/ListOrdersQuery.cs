namespace Order.Application.Queries;

using Order.Domain.Abstraction;
using MediatR;

public class ListOrdersQuery : IRequest<IReadOnlyList<IOrderAnemicModel>>
{
    public Guid CustomerId { get; set; }
}
