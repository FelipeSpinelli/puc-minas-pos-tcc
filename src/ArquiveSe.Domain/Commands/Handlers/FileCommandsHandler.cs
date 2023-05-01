using ArquiveSe.Core.Domain.Models.Entities;
using ArquiveSe.Core.Domain.Models.ValueObjects;
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
            

            return Unit.Value;
        }
    }
}
