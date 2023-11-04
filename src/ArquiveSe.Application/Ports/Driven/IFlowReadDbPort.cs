using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IFlowReadDbPort
{
    Task<FlowProjection> GetFlowById(string id);
}
