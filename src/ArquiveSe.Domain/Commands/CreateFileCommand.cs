using MediatR;

namespace ArquiveSe.Core.Domain.Commands
{
    public class CreateFileCommand : IRequest
    {
        public string AccountId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public Stream FileStream { get; set; } = new MemoryStream();
    }
}
