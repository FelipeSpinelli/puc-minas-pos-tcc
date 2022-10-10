using ArquiveSe.Core.Domain.Shared.Abstractions;
using ArquiveSe.Core.Domain.Shared.Models;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Data.Repositories
{
    internal abstract class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : Entity<TId>
        where TId : struct
    {
        protected readonly IMongoCollection<TEntity> _collection;
        protected abstract string _collectionName { get; }

        protected BaseRepository(MongoClient dbClient)
        {
            _collection = dbClient.GetDatabase("ArquiveSe").GetCollection<TEntity>(_collectionName);
        }

        public async Task<TEntity?> GetById(TId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
            var query = await _collection.FindAsync(filter);
            return await query.FirstOrDefaultAsync();
        }

        public async Task Upsert(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            var existingDocumentQuery = await _collection.FindAsync(filter);
            var existingDocument = await existingDocumentQuery.FirstOrDefaultAsync();

            if (existingDocument != null)
            {
                await _collection.ReplaceOneAsync(filter, entity);
                return;
            }

            await _collection.InsertOneAsync(entity);
        }
    }
}
