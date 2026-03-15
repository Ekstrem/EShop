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
/// Command to approve a return request (Requested -> Approved).
/// </summary>
public sealed class ApproveReturnCommand : IRequest<AggregateResult<IReturnRequest, IReturnRequestAnemicModel>>
{
    public Guid ReturnRequestId { get; init; }
    public string LabelUrl { get; init; } = string.Empty;
    public string Carrier { get; init; } = string.Empty;
}
