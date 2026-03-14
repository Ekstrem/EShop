using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReturnRequest.Application.Commands;
using ReturnRequest.InternalContracts;

namespace ReturnRequest.Api;

/// <summary>
/// API controller for the ReturnRequest bounded context.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ReturnRequestController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IReturnRequestQueryRepository _queryRepository;

    public ReturnRequestController(IMediator mediator, IReturnRequestQueryRepository queryRepository)
    {
        _mediator = mediator;
        _queryRepository = queryRepository;
    }

    [HttpPost]
    public async Task<IActionResult> RequestReturn(
        [FromBody] RequestReturnCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(
        Guid id,
        [FromBody] ApproveReturnRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ApproveReturnCommand
        {
            ReturnRequestId = id,
            LabelUrl = request.LabelUrl,
            Carrier = request.Carrier
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(
        Guid id,
        [FromBody] RejectReturnRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectReturnCommand
        {
            ReturnRequestId = id,
            RejectionReason = request.RejectionReason
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/confirm-shipped")]
    public async Task<IActionResult> ConfirmShipped(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmReturnShippedCommand { ReturnRequestId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/receive")]
    public async Task<IActionResult> Receive(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ReceiveReturnCommand { ReturnRequestId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid id,
        [FromBody] CompleteReturnRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CompleteReturnCommand
        {
            ReturnRequestId = id,
            RefundAmount = request.RefundAmount
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/reject-after-inspection")]
    public async Task<IActionResult> RejectAfterInspection(
        Guid id,
        [FromBody] RejectReturnRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectReturnedItemCommand
        {
            ReturnRequestId = id,
            RejectionReason = request.RejectionReason
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var readModel = await _queryRepository.GetByIdAsync(id, cancellationToken);
        return readModel is null ? NotFound() : Ok(readModel);
    }

    [HttpGet("by-order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        var readModels = await _queryRepository.GetByOrderIdAsync(orderId, cancellationToken);
        return Ok(readModels);
    }

    [HttpGet("by-customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId, CancellationToken cancellationToken)
    {
        var readModels = await _queryRepository.GetByCustomerIdAsync(customerId, cancellationToken);
        return Ok(readModels);
    }

    [HttpGet("by-status/{status}")]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken cancellationToken)
    {
        var readModels = await _queryRepository.GetByStatusAsync(status, cancellationToken);
        return Ok(readModels);
    }

    [HttpGet("by-rma/{rmaNumber}")]
    public async Task<IActionResult> GetByRmaNumber(string rmaNumber, CancellationToken cancellationToken)
    {
        var readModel = await _queryRepository.GetByRmaNumberAsync(rmaNumber, cancellationToken);
        return readModel is null ? NotFound() : Ok(readModel);
    }
}

public sealed class ApproveReturnRequest
{
    public string LabelUrl { get; init; } = string.Empty;
    public string Carrier { get; init; } = string.Empty;
}

public sealed class RejectReturnRequest
{
    public string RejectionReason { get; init; } = string.Empty;
}

public sealed class CompleteReturnRequest
{
    public decimal RefundAmount { get; init; }
}
