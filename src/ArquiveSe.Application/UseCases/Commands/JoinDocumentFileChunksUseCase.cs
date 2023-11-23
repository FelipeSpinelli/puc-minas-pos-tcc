using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Application.UseCases.Commands;

public class JoinDocumentFileChunksUseCase : BaseCommandUseCase<JoinDocumentFileChunksInput, NoOutput>, IJoinDocumentFileChunksUseCase
{
    private readonly IFileStoragePort _fileStorage;

    public JoinDocumentFileChunksUseCase(
        IPersistenceDbPort persistenceDb,
        IFileStoragePort fileStorage)
        : base(persistenceDb)
    {
        _fileStorage = fileStorage;
    }

    public override async Task<NoOutput> Execute(JoinDocumentFileChunksInput input)
    {
        var document = await _persistenceDb.LoadAggregate<Document>(input.Id)
                ?? throw new ApplicationException($"Document {input.Id} was not found!");

        if (!document.File!.IsCompleted)
        {
            throw new ApplicationException($"Document {input.Id} does not have all chunks!");
        }

        await _fileStorage.JoinChunks(document);

        return NoOutput.Value;
    }
}