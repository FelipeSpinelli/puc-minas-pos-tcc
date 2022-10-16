using ArquiveSe.UI.Clients.ArquiveSeApi.Models.Requests;
using ArquiveSe.UI.Clients.ArquiveSeApi.Models.Response;
using RestEase;

namespace ArquiveSe.UI.Abstractions
{
    public interface IArquiveSeApi
    {
        [Post("accounts")]
        Task<Response<CreateAccountResponse>> CreateAccountAsync([Body] CreateAccountRequest request);
    }
}
