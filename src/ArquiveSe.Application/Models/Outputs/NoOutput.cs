namespace ArquiveSe.Application.Models.Outputs;

public record NoOutput
{
    private static NoOutput _value = new();

    public static NoOutput Value => _value;
    public static Task<NoOutput> Task => System.Threading.Tasks.Task.FromResult(_value);
}
