namespace DiscountCode.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IDiscountCode : IBoundedContext { }

public class DiscountCodeBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "DiscountCode";
    public int MicroserviceVersion => 1;
}
