using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Ports.Driving;

public interface ICommandBusPort
{
    Task Send<T>(T message) where T : IBaseRequest;
    Task<TOut> Run<TIn, TOut>(TIn message) where TIn : IRequest<TOut>;
}
