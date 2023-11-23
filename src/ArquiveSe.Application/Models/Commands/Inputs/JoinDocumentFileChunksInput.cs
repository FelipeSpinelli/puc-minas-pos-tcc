using ArquiveSe.Application.Models.Commands.Abstractions;
using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;

public record JoinDocumentFileChunksInput : IRequest<NoOutput>, IIdempotencyCalculator
{
    public string Id { get; set; } = null!;

    public string GetIdempotency() => $"{nameof(JoinDocumentFileChunksInput)}:{Id}";
}