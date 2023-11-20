namespace ArquiveSe.Infra.Storage.Configurations;

public record StorageSettings
{
    public const string SECTION_NAME = nameof(StorageSettings);

    public bool UseLocalStorage { get; set; }
    public string ConnectionStringName { get; set; } = null!;
}
