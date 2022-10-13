using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Models.Requests;
using ArquiveSe.Core.Domain.Commands;
using MediatR;
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

        public async Task<object> Execute(UploadFileRequest request)
        {
            var command = new CreateFileCommand
            {
                AccountId = request.AccountId,
                UserId = request.UserId,
                Name = request.File.FileName,
                FileStream = request.File.OpenReadStream()
            };

            await _bus.Send(command);
            return new();
        }
    }
}
