using DigiTFactory.Libraries.SeedWorks.Result;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Definition;
using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using EShop.Contracts;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Command to reject a returned item after inspection (Received -> RejectedAfterInspection).
/// </summary>
public sealed class RejectReturnedItemCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid ReturnRequestId { get; init; }
    public string RejectionReason { get; init; } = string.Empty;
}
