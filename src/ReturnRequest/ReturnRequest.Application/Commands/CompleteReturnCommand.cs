using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Command to complete a return and issue refund (Received -> Completed).
/// </summary>
public sealed class CompleteReturnCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid ReturnRequestId { get; init; }
    public decimal RefundAmount { get; init; }
}
