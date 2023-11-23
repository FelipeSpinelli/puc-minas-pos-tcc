using ArquiveSe.Application.Models.Commands.Abstractions;
using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;

public record AddDocumentFileChunkInput : IRequest<NoOutput>, IIdempotencyCalculator
{
    public string Id { get; set; } = null!;
    public int Position { get; set; }
    public string Base64Chunk { get; set; } = null!;

    public byte[] GetChuckBytes() => Convert.FromBase64String(Base64Chunk);

    public string GetIdempotency() => $"{nameof(AddDocumentFileChunkInput)}:{Id}:{Position}";
}