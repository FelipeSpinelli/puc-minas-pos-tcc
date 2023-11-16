using ArquiveSe.Infra.Databases.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text;

namespace ArquiveSe.Infra.Databases.Persistence.Configurations;

public class PersistenceDbContext : DbContext
{
    public DbSet<EventDto> Events { get; set; }

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
                .HasColumnType("VARCHAR(MAX)")
                .HasConversion(x => Compress(x), x => Decompress(x));
        });
    }

    private static string Compress(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        using var memoryStream = new MemoryStream();
        using (var brotliStream = new BrotliStream(memoryStream, CompressionLevel.Optimal))
        {
            brotliStream.Write(bytes, 0, bytes.Length);
        }

        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }

    private static string Decompress(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        using var memoryStream = new MemoryStream(bytes);
        using var outputStream = new MemoryStream();
        using (var decompressStream = new BrotliStream(memoryStream, CompressionMode.Decompress))
        {
            decompressStream.CopyTo(outputStream);
        }

        return Encoding.UTF8.GetString(outputStream.ToArray());
    }
}
