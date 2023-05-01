using ArquiveSe.Application.UseCases.Models.Requests;

namespace ArquiveSe.Application.UseCases.Abstractions
{
    public interface IDownloadFileUseCase : IUseCase<DownloadFileRequest, Stream>
    {
    }
}
