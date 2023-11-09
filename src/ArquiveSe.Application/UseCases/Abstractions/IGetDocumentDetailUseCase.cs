using ArquiveSe.Application.Models.Queries.Inputs;
using ArquiveSe.Application.Models.Queries.Outputs;

namespace ArquiveSe.Application.UseCases.Abstractions;

public interface IGetDocumentDetailUseCase : IUseCase<GetDocumentDetailInput, GetDocumentDetailOutput>
{
}