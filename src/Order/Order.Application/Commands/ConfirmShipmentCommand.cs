namespace Order.Application.Commands;

using Hive.SeedWorks.TacticalPatterns;
using Order.Domain;
using Order.Domain.Abstraction;
using MediatR;

public class ConfirmShipmentCommand : IRequest<AggregateResult<IOrder, IOrderAnemicModel>>
{
    public Guid OrderId { get; set; }
}
