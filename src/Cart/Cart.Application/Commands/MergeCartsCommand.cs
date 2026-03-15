namespace Cart.Application.Commands;

using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class MergeCartsCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid TargetCartId { get; set; }
    public Guid SourceCartId { get; set; }
}
