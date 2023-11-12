using ArquiveSe.Application.Models.Projections;
using ArquiveSe.Application.Ports.Driven;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Databases.Read;

public class FolderReadDbAdapter : IFolderReadDbPort
{
    private readonly IMongoCollection<FolderProjection> _collection;

    public FolderReadDbAdapter(IMongoDatabase database)
    {
        _collection = database.GetCollection<FolderProjection>("Folders");
    }

    public async Task<FolderProjection> GetFolderById(string id)
    {
        var cursor = await _collection
            .FindAsync(x => x.Id == id);

        return await cursor.FirstOrDefaultAsync();
    }
}
