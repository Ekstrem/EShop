using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Command to confirm that the return has been shipped (Approved -> ReturnShipped).
/// </summary>
public sealed class ConfirmReturnShippedCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid ReturnRequestId { get; init; }
}
