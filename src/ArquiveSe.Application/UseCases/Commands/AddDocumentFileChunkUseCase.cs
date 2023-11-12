using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;
using System.Buffers;

namespace ArquiveSe.Application.UseCases.Commands;
public class AddDocumentFileChunkUseCase : BaseCommandUseCase<AddDocumentFileChunkInput, NoOutput>, IAddDocumentFileChunkUseCase
{
    private readonly IFileStoragePort _fileStorage;
    private readonly IOperationLockerPort _operationLocker;

    public AddDocumentFileChunkUseCase(
        IPersistenceDbPort persistenceDb,
        IFileStoragePort fileStorage,
        IOperationLockerPort operationLocker)
        : base(persistenceDb)
    {
        _fileStorage = fileStorage;
        _operationLocker = operationLocker;
    }

    public override async Task<NoOutput> Execute(AddDocumentFileChunkInput input)
    {
        var document = await _persistenceDb.LoadAggregate<Document>(input.Id)
            ?? throw new ApplicationException($"Document {input.Id} was not found!");

        var lockerKey = $"{nameof(Document)}:{document.Id}";
        if (!await _operationLocker.TryLock(lockerKey))
        {
            throw new ApplicationException($"Document {document.Id} is locked!");
        }

        var stream = await BuildStream(document, input);
        await _fileStorage.Save(document, stream);

        document.UpdateFileCurrentSize((ulong)stream.Length);

        await PersistEventsOf(document);

        await _operationLocker.Unlock(lockerKey);

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
