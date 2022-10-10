namespace ArquiveSe.Core.Domain.Models.ValueObjects
{
    public class Identity<TId> where TId : struct
    {
        public TId Value { get; private set; }

        public Identity()
        {
        }

        public Identity(TId value)
        {
            Value = value;
        }

        public static implicit operator Identity<TId>(TId value) => new(value);
    }
}
