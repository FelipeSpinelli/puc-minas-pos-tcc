using MediatR;

namespace ArquiveSe.Application.UseCases.Abstractions;

public interface IUseCase<TInput, TOutput>
{
    Task<TOutput> Execute(TInput input);
}
public interface ICommandUseCase<TInput, TOutput> : IUseCase<TInput, TOutput>
    where TInput : IRequest<TOutput>
{
}
