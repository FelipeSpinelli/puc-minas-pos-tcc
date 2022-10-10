using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Core.Domain.Shared.Abstractions;

namespace ArquiveSe.Domain.Abstractions.Repositories
{
    public interface IManagedFileRepository : IRepository<ManagedFile, Guid>
    {
    }
}
