namespace Cart.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class MergeCartsCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid TargetCartId { get; set; }
    public Guid SourceCartId { get; set; }
}
