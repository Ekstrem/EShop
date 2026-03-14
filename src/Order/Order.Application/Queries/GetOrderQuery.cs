namespace Order.Application.Queries;

using Order.Domain.Abstraction;
using MediatR;

public class GetOrderQuery : IRequest<IOrderAnemicModel?>
{
    public Guid OrderId { get; set; }
}
