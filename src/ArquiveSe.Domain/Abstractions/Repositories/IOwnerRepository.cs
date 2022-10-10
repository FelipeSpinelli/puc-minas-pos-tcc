using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Core.Domain.Shared.Abstractions;

namespace ArquiveSe.Domain.Abstractions.Repositories
{
    public interface IOwnerRepository : IRepository<Owner, Guid>
    {
        Task<Owner> GetByExternalId(string externalId);
    }
}
