using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using MediatR;
using ReturnRequest.Domain;
using ReturnRequest.Domain.Abstraction;

namespace ReturnRequest.Application.Commands;

/// <summary>
/// Command to reject a return request (Requested -> Rejected). Reason required.
/// </summary>
public sealed class RejectReturnCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid ReturnRequestId { get; init; }
    public string RejectionReason { get; init; } = string.Empty;
}
