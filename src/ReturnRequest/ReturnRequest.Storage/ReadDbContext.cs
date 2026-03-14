using Microsoft.EntityFrameworkCore;
using ReturnRequest.Storage.Entities;

namespace ReturnRequest.Storage;

/// <summary>
/// Read-only database context for the ReturnRequest bounded context.
/// </summary>
public sealed class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
    }

    public DbSet<ReturnRequestEntity> ReturnRequests => Set<ReturnRequestEntity>();
    public DbSet<ReturnItemEntity> ReturnItems => Set<ReturnItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReturnRequestEntity>(entity =>
        {
            entity.ToTable("ReturnRequests");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.ReturnRequestId);
        });

        modelBuilder.Entity<ReturnItemEntity>(entity =>
        {
            entity.ToTable("ReturnItems");
            entity.HasKey(e => e.Id);
        });
    }
}
