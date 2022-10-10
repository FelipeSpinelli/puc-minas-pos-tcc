using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public abstract class User : Entity<Guid>
{
    public string ExternalId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    protected User() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    protected User(string externalId, string name, Email email) : this()
    {
        ExternalId = externalId;
        Name = name;
        Email = email;
    }
}