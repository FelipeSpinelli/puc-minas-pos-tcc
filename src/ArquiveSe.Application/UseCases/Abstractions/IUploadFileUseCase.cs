using Microsoft.AspNetCore.Http;

namespace ArquiveSe.Application.UseCases.Abstractions
{
    public interface IUploadFileUseCase : IUseCase<HttpRequest, object>
    {
    }
}
