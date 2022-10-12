using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Data.Repositories
{
    internal class ManagedFileRepository : BaseRepository<ManagedFile, Guid>, IManagedFileRepository
    {
        private const string MANAGEDFILES_COLLECTION_NAME = "managedFiles";

        protected override string _collectionName => MANAGEDFILES_COLLECTION_NAME;

        public ManagedFileRepository(MongoClient dbClient) : base(dbClient)
        {
        }
    }
}
