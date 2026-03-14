namespace StockItem.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface IStockItem : IBoundedContext { }

public class StockItemBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "StockItem";
    public int MicroserviceVersion => 1;
}
