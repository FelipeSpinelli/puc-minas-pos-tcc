namespace ArquiveSe.Domain.ValueObjects;

public record Permissions(string[] Reviewers, string[] Approvers)
{
    private static Permissions _empty = new(Array.Empty<string>(), Array.Empty<string>());
    public static Permissions Empty => _empty;
};

public record FileData
{
    public ulong ExpectedSize { get; set; }
    public ulong CurrentSize { get; set; }
    public bool IsCompleted => ExpectedSize == CurrentSize;
}
