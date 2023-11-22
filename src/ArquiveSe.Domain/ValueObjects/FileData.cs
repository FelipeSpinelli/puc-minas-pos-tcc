namespace ArquiveSe.Domain.ValueObjects;

public record FileData
{
    public ulong ExpectedChunks { get; set; }
    public long CurrentChunks { get; set; }
    public ulong ExpectedSize { get; set; }
    public ulong CurrentSize { get; set; }
    public bool IsCompleted => (long)ExpectedChunks == CurrentChunks;
}
