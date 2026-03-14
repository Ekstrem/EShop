namespace Cart.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface ICart : IBoundedContext { }

public class CartBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Cart";
    public int MicroserviceVersion => 1;
}
