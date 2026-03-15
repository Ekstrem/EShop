namespace Cart.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class ApplyPromoCodeCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid CartId { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
}
