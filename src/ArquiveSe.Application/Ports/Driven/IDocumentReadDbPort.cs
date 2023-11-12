using ArquiveSe.Application.Models.Projections;
using System.Linq.Expressions;

namespace ArquiveSe.Application.Ports.Driven;
public interface IDocumentReadDbPort
{
    Task Upsert(DocumentProjection document);
    Task<DocumentProjection> GetDocumentById(string id);
    Task<(IEnumerable<DocumentProjection> Documents, ushort Count)> GetMasterList(Expression<Func<DocumentProjection, bool>> query, ushort offset, ushort size);
}