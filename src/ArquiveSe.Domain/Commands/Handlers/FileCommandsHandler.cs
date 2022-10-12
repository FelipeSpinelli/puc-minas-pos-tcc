using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Domain.Abstractions.Repositories;
using MediatR;

namespace ArquiveSe.Core.Domain.Commands.Handlers
{
    public class FileCommandsHandler : 
        IRequestHandler<CreateFileCommand>
    {
        private readonly IManagedFileRepository _managedFileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBlobRepository _blobRepository;

        public FileCommandsHandler(
            IManagedFileRepository managedFileRepository,
            IUserRepository userRepository,
            IBlobRepository blobRepository)
        {
            _managedFileRepository = managedFileRepository;
            _userRepository = userRepository;
            _blobRepository = blobRepository;
        }

        public async Task<Unit> Handle(CreateFileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByExternalId(request.UserId);
            var managedFile = new ManagedFile(request.Name, user.GetManagedFileOwner());
            await _blobRepository.UploadFile(managedFile.Id.Value.ToString(), request.FileStream);
            await _managedFileRepository.Upsert(managedFile);

            return Unit.Value;
        }
    }
}
