namespace Cart.Application.Commands;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class PlaceOrderCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid CartId { get; set; }
}
