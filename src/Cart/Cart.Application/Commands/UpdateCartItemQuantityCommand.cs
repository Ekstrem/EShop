namespace Cart.Application.Commands;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class UpdateCartItemQuantityCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid CartId { get; set; }
    public Guid VariantId { get; set; }
    public int NewQuantity { get; set; }
}
