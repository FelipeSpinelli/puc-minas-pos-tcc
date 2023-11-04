using ArquiveSe.Application.Models.Inputs;
using ArquiveSe.Application.Models.Outputs;

namespace ArquiveSe.Application.UseCases.Abstractions;

public interface IAddDocumentFileChunkUseCase : IUseCase<AddDocumentFileChunkInput, NoOutput>
{
}