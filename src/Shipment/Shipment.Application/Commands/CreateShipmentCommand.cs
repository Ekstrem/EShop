using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;
using Shipment.Domain;
using Shipment.Domain.Abstraction;

namespace Shipment.Application.Commands;

/// <summary>
/// Command to create a shipment when an order is confirmed.
/// </summary>
public sealed class CreateShipmentCommand : IRequest<AggregateResult<IShipment, IShipmentAnemicModel>>
{
    public Guid OrderId { get; init; }
    public string Carrier { get; init; } = string.Empty;
    public string ShippingAddress { get; init; } = string.Empty;
    public IReadOnlyList<CreateShipmentItemDto> Items { get; init; } = new List<CreateShipmentItemDto>();
}

public sealed class CreateShipmentItemDto
{
    public Guid VariantId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
}
