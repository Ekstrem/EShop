namespace Campaign.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<CampaignEntity> Campaigns => Set<CampaignEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CampaignEntity>(entity =>
        {
            entity.ToTable("campaigns");
            entity.HasKey(e => e.Id);
            entity.HasNoKey();
        });
    }
}
