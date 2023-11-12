using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Shared;
using MediatR;

namespace ArquiveSe.Application.UseCases;

public abstract class BaseCommandUseCase<TInput, TOutput> : 
    ICommandUseCase<TInput, TOutput>, 
    IRequestHandler<TInput, TOutput> 
    where TInput : IRequest<TOutput>
{
    protected readonly IPersistenceDbPort _persistenceDb;

    protected BaseCommandUseCase(IPersistenceDbPort persistenceDb)
    {
        _persistenceDb = persistenceDb;
    }

    protected async Task PersistEventsOf(AggregateRoot aggregateRoot)
    {
        while (aggregateRoot!.TryGetEvent(out var @event))
        {
            await _persistenceDb.AddEvent(@event!);
        }

        await _persistenceDb.SaveAndNotifyEvents();
    }

    public Task<TOutput> Handle(TInput request, CancellationToken cancellationToken) => Execute(request);

    public abstract Task<TOutput> Execute(TInput input);
}