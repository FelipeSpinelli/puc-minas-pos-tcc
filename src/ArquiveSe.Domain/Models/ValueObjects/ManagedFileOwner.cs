using ArquiveSe.Core.Domain.Models.Enumerators;

namespace ArquiveSe.Core.Domain.Models.ValueObjects
{
    public class ManagedFileOwner
    {
        public AccountType Type { get; private set; } = AccountType.Odd;
        public string AccountId { get; private set; } = null!;

        public ManagedFileOwner()
        {
        }

        public ManagedFileOwner(AccountType type, string accountId)
        {
            Type = type;
            AccountId = accountId;
        }
    }
}
