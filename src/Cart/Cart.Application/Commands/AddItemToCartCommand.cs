namespace Cart.Application.Commands;

using Hive.SeedWorks.TacticalPatterns;
using Cart.Domain;
using Cart.Domain.Abstraction;
using MediatR;

public class AddItemToCartCommand : IRequest<AggregateResult<ICart, ICartAnemicModel>>
{
    public Guid CartId { get; set; }
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
