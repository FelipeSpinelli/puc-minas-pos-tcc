using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Application.UseCases.Commands;
public class AddDocumentFileChunkUseCase : BaseCommandUseCase<AddDocumentFileChunkInput, NoOutput>, IAddDocumentFileChunkUseCase
{
    private readonly IFileStoragePort _fileStorage;

    public AddDocumentFileChunkUseCase(
        IPersistenceDbPort persistenceDb,
        IFileStoragePort fileStorage)
        : base(persistenceDb)
    {
        _fileStorage = fileStorage;
    }

    public override async Task<NoOutput> Execute(AddDocumentFileChunkInput input)
    {
        Document? document;

        var retry = -1;
        do
        {
            await Task.Delay(++retry * 100);

            document = await _persistenceDb.LoadAggregate<Document>(input.Id)
                ?? throw new ApplicationException($"Document {input.Id} was not found!");
        }
        while (!document.CanApplyChunk((ulong)input.Position));

        var stream = input.GetChuckBytes();
        await _fileStorage.SaveChunk(document, input.Position, stream);

        document.UpdateFile((ulong)input.Position, (ulong)stream.Length);

        await PersistEventsOf(document);
        return NoOutput.Value;
    }
}