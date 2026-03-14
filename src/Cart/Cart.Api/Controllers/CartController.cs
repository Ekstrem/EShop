namespace Cart.Api.Controllers;

using Cart.Application.Commands;
using Cart.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{cartId:guid}")]
    public async Task<IActionResult> GetCart(Guid cartId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCartQuery { CartId = cartId }, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("customer/{customerId:guid}/active")]
    public async Task<IActionResult> GetActiveCartByCustomer(Guid customerId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new GetActiveCartByCustomerQuery { CustomerId = customerId }, ct);
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost("{cartId:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid cartId,
        [FromBody] AddItemToCartCommand command,
        CancellationToken ct)
    {
        command.CartId = cartId;
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{cartId:guid}/items/{variantId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid cartId, Guid variantId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new RemoveItemFromCartCommand { CartId = cartId, VariantId = variantId }, ct);
        return Ok(result);
    }

    [HttpPut("{cartId:guid}/items/{variantId:guid}/quantity")]
    public async Task<IActionResult> UpdateItemQuantity(
        Guid cartId,
        Guid variantId,
        [FromBody] UpdateCartItemQuantityCommand command,
        CancellationToken ct)
    {
        command.CartId = cartId;
        command.VariantId = variantId;
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPost("{cartId:guid}/promo")]
    public async Task<IActionResult> ApplyPromoCode(
        Guid cartId,
        [FromBody] ApplyPromoCodeCommand command,
        CancellationToken ct)
    {
        command.CartId = cartId;
        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpDelete("{cartId:guid}/promo")]
    public async Task<IActionResult> RemovePromoCode(Guid cartId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new RemovePromoCodeCommand { CartId = cartId }, ct);
        return Ok(result);
    }

    [HttpPost("{targetCartId:guid}/merge/{sourceCartId:guid}")]
    public async Task<IActionResult> MergeCarts(
        Guid targetCartId,
        Guid sourceCartId,
        CancellationToken ct)
    {
        var result = await _mediator.Send(
            new MergeCartsCommand { TargetCartId = targetCartId, SourceCartId = sourceCartId }, ct);
        return Ok(result);
    }

    [HttpPost("{cartId:guid}/checkout")]
    public async Task<IActionResult> PlaceOrder(Guid cartId, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new PlaceOrderCommand { CartId = cartId }, ct);
        return Ok(result);
    }
}
