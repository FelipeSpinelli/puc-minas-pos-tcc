using ArquiveSe.Core.Domain.Models.Enumerators;
using ArquiveSe.Core.Domain.Models.ValueObjects;

namespace ArquiveSe.Core.Domain.Models.Entities;

public class Owner : User
{
    public OwnerType Type { get; private set; } = OwnerType.Odd;

    public Owner() : base()
    {
    }

    public Owner(string externalId, string name, Email email) :
        base(externalId, name, email)
    {
    }
}
