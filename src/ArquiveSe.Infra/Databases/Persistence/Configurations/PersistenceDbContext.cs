using ArquiveSe.Infra.Databases.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace ArquiveSe.Infra.Databases.Persistence.Configurations;

public class PersistenceDbContext : DbContext
{
    public DbSet<EventDto> Events { get; set; }
    public DbSet<IdempotencyDto> Idempotencies { get; set; }

    public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventDto>(builder =>
        {
            builder.HasKey(e => e.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.EventType)
                .HasMaxLength(200);

            builder.Property(x => x.AggregateType)
                .HasMaxLength(200);

            builder.Property(x => x.AggregateId)
                .HasMaxLength(50);

            builder.Property(x => x.Data)
                .IsRequired()
                .HasColumnType("VARCHAR(MAX)");
        });

        modelBuilder.Entity<IdempotencyDto>(builder =>
        {
            builder.HasKey(i => i.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.ValueType)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Data)                
                .IsRequired()
                .HasColumnType("VARCHAR(MAX)");
        });
    }
}
