using ArquiveSe.App.Models.Requests;

namespace ArquiveSe.App.Services.Abstractions;

public interface IArquiveSeApiService
{
    Task CreateDocument(CreateDocumentRequest request);
}
