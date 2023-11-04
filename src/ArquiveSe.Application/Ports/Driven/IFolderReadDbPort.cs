using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IFolderReadDbPort
{
    Task<FolderProjection> GetFolderById(string id);
}
