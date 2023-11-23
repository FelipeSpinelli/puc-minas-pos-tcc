using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Buffers;

namespace ArquiveSe.Infra.Storage;

public class LocalFileStorageAdapter : IFileStoragePort
{
    private readonly ILogger<LocalFileStorageAdapter> _logger;

    public LocalFileStorageAdapter(ILogger<LocalFileStorageAdapter> logger)
    {
        _logger = logger;
    }

    public async Task SaveChunk(Document document, int position, byte[] stream)
    {
        var path = GetDocumentFilePath(document, position);
        _logger.LogInformation($"Adding document {document.Id} chunk #{position} [{path}]");
        using var fs = File.OpenWrite(path)!;
        await fs.WriteAsync(stream);
    }

    public async Task JoinChunks(Document document)
    {
        const int BLOCK_SIZE = 4096;

        _logger.LogInformation($"Joining document {document.Id} chunks");

        var filesPaths = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Files"), $"{document.Id}*.*");
        using var documentFile = File.OpenWrite(GetDocumentFilePath(document))!;
        var arrayPool = ArrayPool<byte>.Shared;
        foreach (var filePath in filesPaths)
        {
            using var fileStream = File.OpenRead(filePath)!;
            var bytes = arrayPool.Rent(BLOCK_SIZE);
            var bytesRead = await fileStream.ReadAsync(bytes, 0, BLOCK_SIZE);
            await documentFile.WriteAsync(bytes, 0, bytesRead);
            arrayPool.Return(bytes, true);
        }
    }

    public async Task<byte[]> Load(Document document)
    {
        using var ms = new MemoryStream();
        using var fs = File.OpenRead(GetDocumentFilePath(document));
        await fs.CopyToAsync(ms);
        return ms.ToArray();
    }

    private static string GetDocumentFilePath(Document document) => Path.Combine(Directory.GetCurrentDirectory(), "Files", document.BuildIdFromDocument());
    private static string GetDocumentFilePath(Document document, int position) => Path.Combine(Directory.GetCurrentDirectory(), "Files", document.BuildIdFromDocument(position));
}
