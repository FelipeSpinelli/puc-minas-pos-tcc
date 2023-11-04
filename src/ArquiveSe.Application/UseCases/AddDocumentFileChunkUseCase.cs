using ArquiveSe.Application.Models.Inputs;
using ArquiveSe.Application.Models.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;
using System.Buffers;

namespace ArquiveSe.Application.UseCases;

public class AddDocumentFileChunkUseCase : BaseUseCase, IAddDocumentFileChunkUseCase
{
    private readonly IFileStoragePort _fileStorage;

    public AddDocumentFileChunkUseCase(
        IPersistenceDbPort persistenceDb,
        IFileStoragePort fileStorage)
        : base(persistenceDb)
    {
        _fileStorage = fileStorage;
    }

    public async Task<NoOutput> Execute(AddDocumentFileChunkInput input)
    {
        var document = await _persistenceDb.LoadAggregate<Document>(input.Id)
            ?? throw new ApplicationException($"Document {input.Id} was not found!");

        var stream = await BuildStream(document, input);
        await _fileStorage.Save(document, stream);

        document.UpdateFileCurrentSize((ulong)stream.Length);

        await PersistEventsOf(document);

        return NoOutput.Value;
    }

    private async Task<byte[]> BuildStream(Document document, AddDocumentFileChunkInput input)
    {
        var stream = await _fileStorage.Load(document);
        var chunk = input.GetChuckBytes();

        if (stream.Length >= input.Position + chunk.Length)
        {
            chunk.CopyTo(stream, input.Position);
            return stream;
        }

        var streamPool = ArrayPool<byte>.Shared;
        var length = stream.Length + chunk.Length;
        var newStream = streamPool.Rent(length);

        stream.CopyTo(newStream, 0);
        chunk.CopyTo(newStream, input.Position);

        stream = newStream.Take(length).ToArray();
        streamPool.Return(newStream, true);

        return stream;
    }
}
