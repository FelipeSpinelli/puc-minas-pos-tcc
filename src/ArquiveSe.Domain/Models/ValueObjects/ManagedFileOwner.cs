using ArquiveSe.Core.Domain.Models.Enumerators;

namespace ArquiveSe.Core.Domain.Models.ValueObjects
{
    public class ManagedFileOwner
    {
        private static ManagedFileOwner _empty = new(AccountType.None);
        public AccountType Type { get; private set; } = AccountType.None;
        public string? AccountId { get; private set; }

        public static ManagedFileOwner Empty => _empty;

        public ManagedFileOwner()
        {
        }

        public ManagedFileOwner(AccountType type, string? accountId = null)
        {
            Type = type;
            AccountId = accountId;
        }

    }
}
