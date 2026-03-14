using Microsoft.EntityFrameworkCore;

namespace Session.Storage;

/// <summary>
/// EF Core DbContext for Session command-side persistence.
/// </summary>
public sealed class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<SessionEntity> Sessions => Set<SessionEntity>();
    public DbSet<LoginAttemptEntity> LoginAttempts => Set<LoginAttemptEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionEntity>(e =>
        {
            e.ToTable("Sessions");
            e.HasKey(s => s.Id);
            e.Property(s => s.Token).HasMaxLength(512).IsRequired();
            e.HasIndex(s => s.Token).IsUnique();
            e.Property(s => s.Status).HasMaxLength(32).IsRequired();
            e.Property(s => s.DeviceInfo).HasMaxLength(512);
            e.HasIndex(s => new { s.CustomerId, s.Status });
        });

        modelBuilder.Entity<LoginAttemptEntity>(e =>
        {
            e.ToTable("LoginAttempts");
            e.HasKey(la => la.Id);
            e.HasIndex(la => new { la.CustomerId, la.AttemptedAt });
        });
    }
}

public sealed class SessionEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string DeviceInfo { get; set; } = string.Empty;
}

public sealed class LoginAttemptEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsSuccessful { get; set; }
    public DateTime AttemptedAt { get; set; }
}
