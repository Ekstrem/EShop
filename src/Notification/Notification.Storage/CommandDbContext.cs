namespace Notification.Storage;

using Microsoft.EntityFrameworkCore;

public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }

    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.CustomerId }).IsUnique();
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
        });
    }
}

public sealed class NotificationEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid EventId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string TemplateId { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public string RenderedContent { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
