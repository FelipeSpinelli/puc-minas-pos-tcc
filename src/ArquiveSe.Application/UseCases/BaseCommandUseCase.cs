using ArquiveSe.Application.Models.Commands.Abstractions;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Shared;
using MediatR;

namespace ArquiveSe.Application.UseCases;

public abstract class BaseCommandUseCase<TInput, TOutput> : 
    ICommandUseCase<TInput, TOutput>, 
    IRequestHandler<TInput, TOutput> 
    where TInput : IRequest<TOutput>, IIdempotencyCalculator
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

    public async Task<TOutput> Execute(TInput input)
    {
        var idempotencyKey = input.GetIdempotency();
        var idempotencyOutput = await _persistenceDb.GetByIdempotency<TOutput>(idempotencyKey);
        if (idempotencyOutput is not null)
        {
            Console.WriteLine("Output got from idempotency");
            return idempotencyOutput;
        }

        var output = await InternalExecute(input);
        await _persistenceDb.AddIdempotency(idempotencyKey, output);

        return output;
    }

    protected abstract Task<TOutput> InternalExecute(TInput input);
}