using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class ManagedFile : Entity<Guid>
{
    public string Name { get; private set; } = null!;
    public ManagedFileOwner Owner { get; private set; } = new();

    public ManagedFile() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public ManagedFile(
        string name,
        ManagedFileOwner owner) : this()
    {
        Name = name;
        Owner = owner;
    }

    public Sharing ShareWith(Email userEmail) => new(this, new[] { userEmail });
}
