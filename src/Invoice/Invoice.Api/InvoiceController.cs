namespace Invoice.Api;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Invoice.Application.Commands;
using Invoice.Application.Queries;
using Invoice.InternalContracts;

[ApiController]
[Route("api/[controller]")]
public sealed class InvoiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateInvoice(
        [FromBody] GenerateInvoiceCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPost("{invoiceId:guid}/send")]
    public async Task<IActionResult> SendInvoice(
        Guid invoiceId,
        CancellationToken ct)
    {
        var command = new SendInvoiceCommand { InvoiceId = invoiceId };
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPost("{invoiceId:guid}/resend")]
    public async Task<IActionResult> ResendInvoice(
        Guid invoiceId,
        CancellationToken ct)
    {
        var command = new ResendInvoiceCommand { InvoiceId = invoiceId };
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpPost("{invoiceId:guid}/credit-note")]
    public async Task<IActionResult> GenerateCreditNote(
        Guid invoiceId,
        [FromBody] GenerateCreditNoteRequest request,
        CancellationToken ct)
    {
        var command = new GenerateCreditNoteCommand
        {
            OriginalInvoiceId = invoiceId,
            CreditNoteNumber = request.CreditNoteNumber,
            RefundAmount = request.RefundAmount
        };

        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(result.ErrorMessage);

        return Ok(result.Model);
    }

    [HttpGet("{invoiceId:guid}")]
    public async Task<IActionResult> GetInvoice(
        Guid invoiceId,
        CancellationToken ct)
    {
        var query = new GetInvoiceByIdQuery { InvoiceId = invoiceId };
        var result = await _mediator.Send(query, ct);
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> ListInvoicesByCustomer(
        Guid customerId,
        CancellationToken ct)
    {
        var query = new ListInvoicesByCustomerQuery { CustomerId = customerId };
        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}

public sealed class GenerateCreditNoteRequest
{
    public string CreditNoteNumber { get; init; } = string.Empty;
    public decimal RefundAmount { get; init; }
}
