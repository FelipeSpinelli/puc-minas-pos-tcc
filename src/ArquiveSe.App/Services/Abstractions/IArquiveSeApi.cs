using ArquiveSe.App.Models.Responses;
using RestEase;

namespace ArquiveSe.App.Services.Abstractions;

public interface IArquiveSeApi
{
    [Post("Document")]
    Task CreateDocument([Body] HttpContent content, [Header("Authorization")] string token);

    [Get("Document")]
    Task<Response<GetMasterListResponse>> GetMasterList();

    [Get("Document/{id}")]
    Task<Response<GetDocumentByIdResponse>> GetDocumentById([Path] string id);
}
