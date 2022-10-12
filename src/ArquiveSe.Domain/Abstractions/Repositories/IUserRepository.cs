using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Core.Domain.Shared.Abstractions;

namespace ArquiveSe.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User> GetByExternalId(string externalId);
    }
}
