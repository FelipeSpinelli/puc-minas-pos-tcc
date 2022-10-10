namespace ArquiveSe.Application.UseCases.Abstractions
{
    public interface IUseCase<TRequest, TResponse>
    {
        Task<TResponse> Execute(TRequest request);
    }
}
