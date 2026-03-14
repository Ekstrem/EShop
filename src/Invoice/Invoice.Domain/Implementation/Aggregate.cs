namespace Invoice.Domain.Implementation;

using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Specifications;

internal sealed class Aggregate : Aggregate<IInvoice, IInvoiceAnemicModel>
{
    private Aggregate(IInvoiceAnemicModel model) : base(model) { }

    public static Aggregate CreateInstance(IInvoiceAnemicModel model) => new(model);

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateInvoice(
        string invoiceNumber,
        Guid orderId,
        Guid customerId,
        IReadOnlyList<IInvoiceLine> lines)
    {
        var fieldsValidator = new HasRequiredFieldsValidator();
        if (!fieldsValidator.IsSatisfiedBy(invoiceNumber, customerId, lines))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Invoice must have a number, customer, and at least one line with VAT.");

        var sequentialValidator = new SequentialNumberValidator();
        if (!sequentialValidator.IsSatisfiedBy(invoiceNumber))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Invoice number must follow the sequential format.");

        var root = InvoiceRoot.CreateInstance(
            invoiceNumber,
            orderId,
            customerId,
            DateTime.UtcNow,
            "Generated",
            "Invoice");

        var totals = InvoiceTotals.CalculateFromLines(lines);

        var anemic = new AnemicModel
        {
            Root = root,
            Lines = lines.ToList(),
            Totals = totals
        };

        return AggregateResult<IInvoice, IInvoiceAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> SendInvoice()
    {
        var validator = new IsGeneratedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Only invoices in Generated status can be sent.");

        var root = InvoiceRoot.CreateInstance(
            Model.Root.InvoiceNumber,
            Model.Root.OrderId,
            Model.Root.CustomerId,
            Model.Root.IssueDate,
            "Sent",
            Model.Root.InvoiceType);

        var anemic = new AnemicModel
        {
            Root = root,
            Lines = Model.Lines.ToList(),
            Totals = Model.Totals
        };

        return AggregateResult<IInvoice, IInvoiceAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> ResendInvoice()
    {
        var validator = new IsSentValidator();
        if (!validator.IsSatisfiedBy(Model))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Only invoices in Sent status can be resent.");

        var root = InvoiceRoot.CreateInstance(
            Model.Root.InvoiceNumber,
            Model.Root.OrderId,
            Model.Root.CustomerId,
            Model.Root.IssueDate,
            "Sent",
            Model.Root.InvoiceType);

        var anemic = new AnemicModel
        {
            Root = root,
            Lines = Model.Lines.ToList(),
            Totals = Model.Totals
        };

        return AggregateResult<IInvoice, IInvoiceAnemicModel>.Ok(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateCreditNote(
        string creditNoteNumber,
        decimal refundAmount)
    {
        var immutableValidator = new InvoiceImmutableValidator();
        if (!immutableValidator.IsSatisfiedBy(Model))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Cannot create a credit note for an invoice that is not yet generated.");

        var sequentialValidator = new SequentialNumberValidator();
        if (!sequentialValidator.IsSatisfiedBy(creditNoteNumber))
            return AggregateResult<IInvoice, IInvoiceAnemicModel>.Fail(
                "Credit note number must follow the sequential format.");

        var creditLine = InvoiceLine.CreateInstance(
            $"Credit note for invoice {Model.Root.InvoiceNumber}",
            1,
            -refundAmount,
            Model.Lines.Any() ? Model.Lines.First().VatRate : 0m);

        var lines = new List<IInvoiceLine> { creditLine };
        var totals = InvoiceTotals.CalculateFromLines(lines);

        var root = InvoiceRoot.CreateInstance(
            creditNoteNumber,
            Model.Root.OrderId,
            Model.Root.CustomerId,
            DateTime.UtcNow,
            "Generated",
            "CreditNote");

        var anemic = new AnemicModel
        {
            Root = root,
            Lines = lines,
            Totals = totals
        };

        return AggregateResult<IInvoice, IInvoiceAnemicModel>.Ok(anemic);
    }
}
