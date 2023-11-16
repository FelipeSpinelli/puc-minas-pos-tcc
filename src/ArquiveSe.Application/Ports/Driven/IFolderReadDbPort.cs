using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IFolderReadDbPort
{
    Task Upsert(FolderProjection folder);
    Task<FolderProjection> GetFolderById(string id);
}
