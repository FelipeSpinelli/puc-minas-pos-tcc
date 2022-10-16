using ArquiveSe.Application.UseCases.Models.Requests;
using ArquiveSe.Application.UseCases.Models.Responses;

namespace ArquiveSe.Application.UseCases.Abstractions
{
    public interface ICreateAccountUseCase : IUseCase<CreateAccountRequest, CreateAccountResponse>
    {
    }
}
