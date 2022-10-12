using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Data.Repositories
{
    internal class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        private const string USERS_COLLECTION_NAME = "users";

        protected override string _collectionName => USERS_COLLECTION_NAME;

        public UserRepository(MongoClient dbClient) : base(dbClient)
        {
        }

        public async Task<User> GetByExternalId(string externalId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.ExternalId, externalId);
            var query = await _collection.FindAsync(filter);
            return await query.FirstOrDefaultAsync();
        }
    }
}
