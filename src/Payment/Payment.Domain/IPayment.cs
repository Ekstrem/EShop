namespace Payment.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IPayment : IBoundedContext { }

public class PaymentBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Payment";
    public int MicroserviceVersion => 1;
}
