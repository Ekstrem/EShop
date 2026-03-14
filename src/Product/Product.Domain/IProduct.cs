namespace Product.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IProduct : IBoundedContext { }

public class ProductBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Product";
    public int MicroserviceVersion => 1;
}
