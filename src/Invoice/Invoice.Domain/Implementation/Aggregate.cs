namespace Invoice.Domain.Implementation;

using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using Invoice.Domain.Abstraction;
using Invoice.Domain.Specifications;
using EShop.Contracts;

internal sealed class Aggregate
{
    public IInvoiceAnemicModel Model { get; }

    private Aggregate(IInvoiceAnemicModel model) => Model = model;

    public static Aggregate CreateInstance(IInvoiceAnemicModel model) => new(model);

    private AggregateResult<IInvoice, IInvoiceAnemicModel> Success(IInvoiceAnemicModel newModel)
    {
        var data = BusinessOperationData<IInvoice, IInvoiceAnemicModel>
            .Commit<IInvoice, IInvoiceAnemicModel>(Model, newModel);
        return new AggregateResultSuccess<IInvoice, IInvoiceAnemicModel>(data);
    }

    private AggregateResult<IInvoice, IInvoiceAnemicModel> Fail(string error)
    {
        var data = BusinessOperationData<IInvoice, IInvoiceAnemicModel>
            .Commit<IInvoice, IInvoiceAnemicModel>(Model, Model);
        return new AggregateResultException<IInvoice, IInvoiceAnemicModel>(
            data, new FailedSpecification<IInvoice, IInvoiceAnemicModel>(error));
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateInvoice(
        string invoiceNumber,
        Guid orderId,
        Guid customerId,
        IReadOnlyList<IInvoiceLine> lines)
    {
        var fieldsValidator = new HasRequiredFieldsValidator();
        if (!fieldsValidator.IsSatisfiedBy(invoiceNumber, customerId, lines))
            return Fail("Invoice must have a number, customer, and at least one line with VAT.");

        var sequentialValidator = new SequentialNumberValidator();
        if (!sequentialValidator.IsSatisfiedBy(invoiceNumber))
            return Fail("Invoice number must follow the sequential format.");

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

        return Success(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> SendInvoice()
    {
        var validator = new IsGeneratedValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only invoices in Generated status can be sent.");

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

        return Success(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> ResendInvoice()
    {
        var validator = new IsSentValidator();
        if (!validator.IsSatisfiedBy(Model))
            return Fail("Only invoices in Sent status can be resent.");

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

        return Success(anemic);
    }

    public AggregateResult<IInvoice, IInvoiceAnemicModel> GenerateCreditNote(
        string creditNoteNumber,
        decimal refundAmount)
    {
        var immutableValidator = new InvoiceImmutableValidator();
        if (!immutableValidator.IsSatisfiedBy(Model))
            return Fail("Cannot create a credit note for an invoice that is not yet generated.");

        var sequentialValidator = new SequentialNumberValidator();
        if (!sequentialValidator.IsSatisfiedBy(creditNoteNumber))
            return Fail("Credit note number must follow the sequential format.");

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

        return Success(anemic);
    }
}
