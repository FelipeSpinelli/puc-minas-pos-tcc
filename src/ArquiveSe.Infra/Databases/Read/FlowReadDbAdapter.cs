using ArquiveSe.Application.Models.Projections;
using ArquiveSe.Application.Ports.Driven;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Databases.Read;

public class FlowReadDbAdapter : IFlowReadDbPort
{
    private readonly IMongoCollection<FlowProjection> _collection;

    public FlowReadDbAdapter(IMongoDatabase database)
    {
        _collection = database.GetCollection<FlowProjection>("Flows");
    }

    public async Task<FlowProjection> GetFlowById(string id)
    {
        var cursor = await _collection
            .FindAsync(x => x.Id == id);

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task Upsert(FlowProjection flow)
    {
        var exists = (await _collection.CountDocumentsAsync(x => x.Id == flow.Id)) != 0;

        await (exists ?
            _collection.FindOneAndReplaceAsync(x => x.Id == flow.Id, flow) :
            _collection.InsertOneAsync(flow));
    }
}
