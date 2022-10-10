using ArquiveSe.Core.Domain.Shared.Models;

namespace ArquiveSe.Core.Domain.Shared.Abstractions
{
    public interface IRepository<TEntity, TId> 
        where TEntity : Entity<TId>
        where TId : struct
    {
        Task<TEntity?> GetById(TId id);
        Task Upsert(TEntity entity);
    }
}
