using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Data.Repositories
{
    internal class OwnerRepository : BaseRepository<Owner, Guid>, IOwnerRepository
    {
        protected override string _collectionName => $"owners";

        public OwnerRepository(MongoClient dbClient) : base(dbClient)
        {
        }

        public async Task<Owner> GetByExternalId(string externalId)
        {
            var filter = Builders<Owner>.Filter.Eq(x => x.ExternalId, externalId);
            var query = await _collection.FindAsync(filter);
            return await query.FirstOrDefaultAsync();
        }
    }
}
