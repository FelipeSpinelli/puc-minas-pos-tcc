using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Core.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ArquiveSe.Application.UseCases
{
    internal class UploadFileUseCase : IUploadFileUseCase
    {
        private readonly IMediator _bus;
        private readonly ILogger<UploadFileUseCase> _logger;

        public UploadFileUseCase(
            IMediator bus,
            ILogger<UploadFileUseCase> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public async Task<object> Execute(HttpRequest request)
        {
            await request.ReadFormAsync();
            var file = request.Form.Files["file"];

            var command = new CreateFileCommand
            {
                Name = file.FileName,
                UserId = Guid.NewGuid().ToString(),
                FileStream = file.OpenReadStream()
            };

            await _bus.Send(command);
            return new();
        }
    }
}
