using EShop.Contracts;
namespace Payment.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Commands;
using Payment.Application.Queries;
using Payment.InternalContracts;

[ApiController]
[Route("api/[controller]")]
public sealed class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment(
        [FromBody] InitiatePaymentCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{paymentId:guid}/webhook")]
    public async Task<IActionResult> HandleWebhook(
        Guid paymentId,
        [FromBody] HandleWebhookRequest request,
        CancellationToken ct)
    {
        var command = new HandleWebhookCommand
        {
            PaymentId = paymentId,
            ProviderTransactionId = request.ProviderTransactionId,
            TransactionType = request.TransactionType,
            Amount = request.Amount,
            TransactionStatus = request.TransactionStatus
        };

        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{paymentId:guid}/void")]
    public async Task<IActionResult> VoidPayment(
        Guid paymentId,
        CancellationToken ct)
    {
        var command = new VoidPaymentCommand { PaymentId = paymentId };
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpPost("{paymentId:guid}/refund")]
    public async Task<IActionResult> RequestRefund(
        Guid paymentId,
        [FromBody] RequestRefundRequest request,
        CancellationToken ct)
    {
        var command = new RequestRefundCommand
        {
            PaymentId = paymentId,
            RefundAmount = request.RefundAmount
        };

        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess())
            return BadRequest(result.ErrorMessage());

        return Ok(result.Model());
    }

    [HttpGet("{paymentId:guid}/status")]
    public async Task<IActionResult> GetPaymentStatus(
        Guid paymentId,
        CancellationToken ct)
    {
        var query = new GetPaymentStatusQuery { PaymentId = paymentId };
        var result = await _mediator.Send(query, ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{paymentId:guid}/transactions")]
    public async Task<IActionResult> ListTransactions(
        Guid paymentId,
        CancellationToken ct)
    {
        var query = new ListTransactionsQuery { PaymentId = paymentId };
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}

public sealed class HandleWebhookRequest
{
    public string ProviderTransactionId { get; init; } = string.Empty;
    public string TransactionType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string TransactionStatus { get; init; } = string.Empty;
}

public sealed class RequestRefundRequest
{
    public decimal RefundAmount { get; init; }
}
