using ArquiveSe.Core.Domain.Commands;
using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class Account : Entity<Guid>
{
    public string Key { get; private set; } = null!;

    public Account() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    public Account(CreateAccountCommand createAccountCommand) : this()
    {
        Key = createAccountCommand.Key;
    }
}