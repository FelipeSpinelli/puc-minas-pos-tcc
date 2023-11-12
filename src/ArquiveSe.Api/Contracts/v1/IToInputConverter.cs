namespace ArquiveSe.Api.Contracts.v1;

public interface IToInputConverter<TInput>
{
    void ToInput(out TInput input);
}