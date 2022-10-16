using ArquiveSe.Core.Domain.Models.Entities;
using MediatR;

namespace ArquiveSe.Core.Domain.Commands
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public string Key { get; private set; } = null!;
    }
}
