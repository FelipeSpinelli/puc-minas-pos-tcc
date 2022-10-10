using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MediatR;

namespace ArquiveSe.Core.Domain.Commands.Handlers
{
    public class FileCommandsHandler : 
        IRequestHandler<CreateFileCommand>
    {
        private readonly IManagedFileRepository _managedFileRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IBlobRepository _blobRepository;

        public FileCommandsHandler(
            IManagedFileRepository managedFileRepository,
            IOwnerRepository ownerRepository,
            IBlobRepository blobRepository)
        {
            _managedFileRepository = managedFileRepository;
            _ownerRepository = ownerRepository;
            _blobRepository = blobRepository;
        }

        public async Task<Unit> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByExternalId(request.UserId);
            var managedFile = new ManagedFile(request.Name, owner);
            await _blobRepository.UploadFile(managedFile.Id.Value.ToString(), request.FileStream);
            await _managedFileRepository.Upsert(managedFile);

            return Unit.Value;
        }
    }
}
