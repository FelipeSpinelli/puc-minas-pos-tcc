using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Application.UseCases.Models.Requests;
using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Core.Domain.Models.ValueObjects;
using ArquiveSe.Domain.Abstractions.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArquiveSe.Application.UseCases
{
    internal class UploadFileUseCase : IUploadFileUseCase
    {
        private readonly ILogger<UploadFileUseCase> _logger;
        private readonly IManagedFileRepository _managedFileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBlobRepository _blobRepository;

        public UploadFileUseCase(
            ILogger<UploadFileUseCase> logger,
            IManagedFileRepository managedFileRepository,
            IUserRepository userRepository,
            IBlobRepository blobRepository)
        {
            _logger = logger;
            _managedFileRepository = managedFileRepository;
            _userRepository = userRepository;
            _blobRepository = blobRepository;
        }

        public async Task<object> Execute(UploadFileRequest request)
        {
            var user = await _userRepository.GetByExternalId(request.UserId);
            var managedFile = new ManagedFile(request.File.FileName, user?.GetManagedFileOwner() ?? ManagedFileOwner.Empty);
            await _blobRepository.UploadFile(managedFile.Id.Value.ToString(), request.File.OpenReadStream());
            await _managedFileRepository.Upsert(managedFile);

            return new
            {
                Id = managedFile.Id.Value,
                managedFile.Expiration
            };
        }
    }
}
