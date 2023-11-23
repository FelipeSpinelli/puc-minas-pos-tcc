using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;

public record JoinDocumentFileChunksInput : IRequest<NoOutput>
{
    public string Id { get; set; } = null!;
}