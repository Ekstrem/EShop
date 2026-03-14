using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Command to request a return for a delivered order (within 14 days).
/// </summary>
public sealed class RequestReturnCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTime OrderDeliveredAt { get; init; }
    public IReadOnlyList<RequestReturnItemDto> Items { get; init; } = new List<RequestReturnItemDto>();
}

public sealed class RequestReturnItemDto
{
    public Guid VariantId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
