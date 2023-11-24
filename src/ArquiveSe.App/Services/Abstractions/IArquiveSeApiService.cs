using ArquiveSe.App.Models.Requests;
using ArquiveSe.App.Models.Responses;

namespace ArquiveSe.App.Services.Abstractions;

public interface IArquiveSeApiService
{
    Task CreateDocument(CreateDocumentRequest request);
    Task<GetMasterListResponse> GetMasterList();
    Task<GetDocumentByIdResponse> GetDocumentById(string id);
}
