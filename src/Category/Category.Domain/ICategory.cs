namespace Category.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface ICategory : IBoundedContext { }

public class CategoryBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Category";
    public int MicroserviceVersion => 1;
}
