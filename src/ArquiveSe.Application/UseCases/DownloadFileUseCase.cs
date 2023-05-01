using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Models.Requests;
using ArquiveSe.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Logging;

namespace ArquiveSe.Application.UseCases
{
    internal class DownloadFileUseCase : IDownloadFileUseCase
    {
        private readonly ILogger<DownloadFileUseCase> _logger;
        private readonly IManagedFileRepository _managedFileRepository;
        private readonly IBlobRepository _blobRepository;

        public DownloadFileUseCase(
            ILogger<DownloadFileUseCase> logger,
            IManagedFileRepository managedFileRepository,
            IBlobRepository blobRepository)
        {
            _logger = logger;
            _managedFileRepository = managedFileRepository;
            _blobRepository = blobRepository;
        }

        public async Task<Stream> Execute(DownloadFileRequest request)
        {
            var managedFile = await _managedFileRepository.GetById(Guid.Parse(request.Id));
            if (managedFile == null)
            {
                throw new FileNotFoundException();
            }

            return await _blobRepository.GetStreamById(managedFile.Id.Value.ToString());
        }
    }
}
