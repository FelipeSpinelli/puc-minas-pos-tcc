using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Ports.Driven;

public interface IAccountManagementPort
{
    Task<IEnumerable<AccountProjection>> ListAccounts(string[] ids);
}
