namespace Order.Application.Commands;

using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using Order.Domain;
using Order.Domain.Abstraction;
using MediatR;

public class ConfirmDeliveryCommand : IRequest<AggregateResult<IOrder, IOrderAnemicModel>>
{
    public Guid OrderId { get; set; }
}
