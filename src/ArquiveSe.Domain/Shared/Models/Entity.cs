using ArquiveSe.Core.Domain.Models.ValueObjects;

namespace ArquiveSe.Core.Domain.Shared.Models
{
    public abstract class Entity<TId> where TId : struct
    {
        public Identity<TId> Id { get; private set; } = new();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        protected Entity() : base()
        {
        }

        protected Entity(Identity<TId> id, DateTime createdAt)
        {
            Id = id;
            CreatedAt = createdAt;
        }
    }
}
