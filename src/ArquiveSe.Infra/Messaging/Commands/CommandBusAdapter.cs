using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Infra.Messaging.Configurations;
using MediatR;

namespace ArquiveSe.Infra.Messaging.Commands;

public abstract class CommandBusAdapter : ICommandBusPort
{
    protected readonly IMediator _bus;
    protected readonly MessagingSettings _settings;

    protected CommandBusAdapter(IMediator bus, MessagingSettings settings)
    {
        _bus = bus;
        _settings = settings;
    }

    public async Task<TOut> Run<TIn, TOut>(TIn message) where TIn : IRequest<TOut> => await _bus.Send(message);

    public abstract Task Send<T>(T message) where T : IBaseRequest;
}