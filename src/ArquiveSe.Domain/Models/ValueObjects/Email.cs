namespace ArquiveSe.Core.Domain.Models.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; } = null!;

        public Email()
        {
        }

        public Email(string value)
        {
            Value = value;
        }

        public static implicit operator Email(string value) => new(value);
    }
}
