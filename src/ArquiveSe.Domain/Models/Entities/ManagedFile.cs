using ArquiveSe.Core.Domain.Models.Enumerators;
using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class ManagedFile : Entity<Guid>
{
    private readonly int NONE_ACCOUNT_EXPIRATION_DAYS = 7;
    public string Name { get; private set; } = null!;
    public ManagedFileOwner Owner { get; private set; } = new();
    public bool IsDownloadableOnce { get; private set; }
    public DateTime? Expiration { get; private set; }

    public ManagedFile() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public ManagedFile(
        string name,
        ManagedFileOwner owner) : this()
    {
        Name = name;
        Owner = owner;
        IsDownloadableOnce = owner.Type == AccountType.None;
        Expiration = owner.Type == AccountType.None 
            ? DateTime.UtcNow.AddDays(NONE_ACCOUNT_EXPIRATION_DAYS) 
            : null;
    }

    public Sharing ShareWith(Email userEmail) => new(this, new[] { userEmail });
}
