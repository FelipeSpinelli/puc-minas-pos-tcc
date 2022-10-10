using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class Sharing : Entity<Guid>
{
    public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddDays(3);
    public IEnumerable<string> AllowedUsers { get; private set; } = Array.Empty<string>();
    public ManagedFile SharedFile { get; private set; } = null!;

    public Sharing() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public Sharing(
        ManagedFile file,
        IEnumerable<Email> allowedUsers,
        DateTime? expiresAt = default) : this()
    {
        SharedFile = file;
        AllowedUsers = allowedUsers.Select(x => x.Value);
        ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(3);
    }
}
