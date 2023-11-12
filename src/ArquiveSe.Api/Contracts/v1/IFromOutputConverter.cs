namespace ArquiveSe.Api.Contracts.v1;

public interface IFromOutputConverter<TOutput>
{
    void From(TOutput output);
}