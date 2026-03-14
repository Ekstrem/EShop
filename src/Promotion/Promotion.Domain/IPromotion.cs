namespace Promotion.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IPromotion : IBoundedContext { }

public class PromotionBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Promotion";
    public int MicroserviceVersion => 1;
}
