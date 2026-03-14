namespace Order.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IOrder : IBoundedContext { }

public class OrderBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Order";
    public int MicroserviceVersion => 1;
}
