using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;

namespace ArquiveSe.Application.UseCases.Abstractions;

public interface IAddDocumentFileChunkUseCase : ICommandUseCase<AddDocumentFileChunkInput, NoOutput>
{
}
