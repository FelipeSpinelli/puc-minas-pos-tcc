using ArquiveSe.Core.Domain.Models.Enumerators;
using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class User : Entity<Guid>
{
    public string AccountId { get; private set; } = null!;
    public string ExternalId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    protected User() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    protected User(string externalId, string name, Email email, string? accountId = default) : this()
    {
        AccountId = accountId ?? string.Empty;
        ExternalId = externalId;
        Name = name;
        Email = email;
    }

    public ManagedFileOwner GetManagedFileOwner()
    {
        if (string.IsNullOrEmpty(AccountId))
        {
            return new(AccountType.Odd, Id.Value.ToString());
        }

        return new(AccountType.Enterprise, AccountId);
    }
}