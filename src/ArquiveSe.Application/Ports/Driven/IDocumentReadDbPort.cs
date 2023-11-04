using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IDocumentReadDbPort
{
    Task<DocumentProjection> GetDocumentById(string id);
}