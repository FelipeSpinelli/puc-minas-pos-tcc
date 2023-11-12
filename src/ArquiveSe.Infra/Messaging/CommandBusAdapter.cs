using ArquiveSe.Application.Ports.Driving;
using MediatR;

namespace ArquiveSe.Infra.Messaging;

public class CommandBusAdapter : ICommandBusPort
{
    private readonly IMediator _bus;

    public CommandBusAdapter(IMediator bus)
    {
        _bus = bus;
    }

    public async Task<TOut> Run<TIn, TOut>(TIn message) where TIn : IRequest<TOut> => await _bus.Send(message);

    public Task Send<T>(T message) where T : IBaseRequest => _bus.Send(message);
}
