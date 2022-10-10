using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MongoDB.Driver;

namespace ArquiveSe.Infra.Data.Repositories
{
    internal class ManagedFileRepository : BaseRepository<ManagedFile, Guid>, IManagedFileRepository
    {
        protected override string _collectionName => $"managedFiles";

        public ManagedFileRepository(MongoClient dbClient) : base(dbClient)
        {
        }
    }
}
