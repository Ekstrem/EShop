using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipment.Application.Commands;
using Shipment.InternalContracts;

namespace Shipment.Api;

/// <summary>
/// API controller for the Shipment bounded context.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class ShipmentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IShipmentQueryRepository _queryRepository;

    public ShipmentController(IMediator mediator, IShipmentQueryRepository queryRepository)
    {
        _mediator = mediator;
        _queryRepository = queryRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/pack")]
    public async Task<IActionResult> MarkAsPacked(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new MarkAsPackedCommand { ShipmentId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/dispatch")]
    public async Task<IActionResult> Dispatch(
        Guid id,
        [FromBody] DispatchShipmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DispatchShipmentCommand
        {
            ShipmentId = id,
            TrackingNumber = request.TrackingNumber,
            LabelUrl = request.LabelUrl
        };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/carrier-update")]
    public async Task<IActionResult> HandleCarrierUpdate(
        Guid id,
        [FromBody] HandleCarrierUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var command = new HandleCarrierUpdateCommand
        {
            ShipmentId = id,
            NewStatus = request.NewStatus
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
        var readModel = await _queryRepository.GetByOrderIdAsync(orderId, cancellationToken);
        return readModel is null ? NotFound() : Ok(readModel);
    }

    [HttpGet("by-status/{status}")]
    public async Task<IActionResult> GetByStatus(string status, CancellationToken cancellationToken)
    {
        var readModels = await _queryRepository.GetByStatusAsync(status, cancellationToken);
        return Ok(readModels);
    }

    [HttpGet("by-carrier/{carrier}")]
    public async Task<IActionResult> GetByCarrier(string carrier, CancellationToken cancellationToken)
    {
        var readModels = await _queryRepository.GetByCarrierAsync(carrier, cancellationToken);
        return Ok(readModels);
    }
}

public sealed class DispatchShipmentRequest
{
    public string TrackingNumber { get; init; } = string.Empty;
    public string LabelUrl { get; init; } = string.Empty;
}

public sealed class HandleCarrierUpdateRequest
{
    public string NewStatus { get; init; } = string.Empty;
}
