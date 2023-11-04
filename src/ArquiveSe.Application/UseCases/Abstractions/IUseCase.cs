namespace ArquiveSe.Application.UseCases.Abstractions;

public interface IUseCase<TInput, TOutput>
{
    Task<TOutput> Execute(TInput input);
}
