using ArquiveSe.Application.Models.Projections;
using ArquiveSe.Application.Ports.Driven;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ArquiveSe.Infra.Databases.Read;

public class DocumentReadDbAdapter : IDocumentReadDbPort
{
    private readonly IMongoCollection<DocumentProjection> _collection;

    public DocumentReadDbAdapter(IMongoDatabase database)
    {
        _collection = database.GetCollection<DocumentProjection>("Documents");
    }

    public async Task<DocumentProjection> GetDocumentById(string id)
    {
        var cursor = await _collection
            .FindAsync(x => x.Id == id);

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<DocumentProjection> Documents, ushort Count)> GetMasterList(Expression<Func<DocumentProjection, bool>> query, ushort offset, ushort size)
    {
        var count = await _collection.CountDocumentsAsync(query);
        var cursor = await _collection
            .Find(query)
            .Skip(offset)
            .Limit(size)
            .ToCursorAsync();

        return (cursor.ToList(), (ushort)count);
    }

    public async Task Upsert(DocumentProjection document)
    {
        var exists = (await _collection.CountDocumentsAsync(x => x.Id == document.Id)) != 0;

        await (exists ?
            _collection.FindOneAndReplaceAsync(x => x.Id == document.Id, document) :
            _collection.InsertOneAsync(document));
    }
}