namespace Invoice.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IInvoice : IBoundedContext { }

public class InvoiceBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Invoice";
    public int MicroserviceVersion => 1;
}
