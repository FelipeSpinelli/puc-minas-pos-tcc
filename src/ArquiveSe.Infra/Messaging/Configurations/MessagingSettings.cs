namespace ArquiveSe.Infra.Messaging.Configurations;

public record MessagingSettings
{
    public const string SECTION_NAME = nameof(MessagingSettings);

    public bool UseInMemory { get; set; }
    public string ConnectionStringName { get; set; } = null!;
}
