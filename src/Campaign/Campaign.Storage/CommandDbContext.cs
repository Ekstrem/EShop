namespace Campaign.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<CampaignEntity> Campaigns => Set<CampaignEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CampaignEntity>(entity =>
        {
            entity.ToTable("campaigns");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ScheduledAt);
        });
    }
}

public sealed class CampaignEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string TemplateId { get; set; } = string.Empty;
    public string SegmentId { get; set; } = string.Empty;
    public DateTime? ScheduledAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalRecipients { get; set; }
    public int SentCount { get; set; }
    public int FailedCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
