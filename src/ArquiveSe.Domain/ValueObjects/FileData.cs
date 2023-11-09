namespace ArquiveSe.Domain.ValueObjects;

public record FileData
{
    public ulong ExpectedSize { get; set; }
    public ulong CurrentSize { get; set; }
    public bool IsCompleted => ExpectedSize == CurrentSize;
}
