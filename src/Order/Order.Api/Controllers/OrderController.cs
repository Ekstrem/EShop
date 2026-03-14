namespace Order.Api.Controllers;

using Order.Application.Commands;
using Order.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrder(Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderQuery { OrderId = orderId }, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> ListOrders(Guid customerId, CancellationToken ct)
    {
        var result = await _mediator.Send(new ListOrdersQuery { CustomerId = customerId }, ct);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/confirm-payment")]
    public async Task<IActionResult> ConfirmPayment(Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new ConfirmPaymentCommand { OrderId = orderId }, ct);
        return Ok(result);
    }

    [HttpPut("{orderId:guid}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CancelOrderCommand { OrderId = orderId }, ct);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/confirm-shipment")]
    public async Task<IActionResult> ConfirmShipment(Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new ConfirmShipmentCommand { OrderId = orderId }, ct);
        return Ok(result);
    }

    [HttpPost("{orderId:guid}/confirm-delivery")]
    public async Task<IActionResult> ConfirmDelivery(Guid orderId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new ConfirmDeliveryCommand { OrderId = orderId }, ct);
        return Ok(result);
    }
}
