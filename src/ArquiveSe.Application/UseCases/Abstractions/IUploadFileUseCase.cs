using ArquiveSe.Application.UseCases.Models.Requests;

namespace ArquiveSe.Application.UseCases.Abstractions
{
    public interface IUploadFileUseCase : IUseCase<UploadFileRequest, object>
    {
    }
}
