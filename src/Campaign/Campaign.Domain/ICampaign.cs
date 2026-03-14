namespace Campaign.Domain;

using Hive.SeedWorks.TacticalPatterns;

public interface ICampaign : IBoundedContext { }

public class CampaignBoundedContextDescription : IBoundedContextDescription
{
    public string ContextName => "Campaign";
    public int MicroserviceVersion => 1;
}
