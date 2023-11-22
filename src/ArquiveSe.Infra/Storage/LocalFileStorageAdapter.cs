using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;
using System.Buffers;

namespace ArquiveSe.Infra.Storage;

public class LocalFileStorageAdapter : IFileStoragePort
{
    public async Task SaveChunk(Document document, int position, byte[] stream)
    {
        using var fs = File.OpenWrite(GetDocumentFilePath(document, position))!;
        await fs.WriteAsync(stream);
    }

    public async Task JoinChunks(Document document)
    {
        const int BLOCK_SIZE = 4096;
        var filesPaths = Directory.GetFiles("Files", $"{document.Id}*.*");
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

    private static string GetDocumentFilePath(Document document) => Path.Combine("Files", document.BuildIdFromDocument());
    private static string GetDocumentFilePath(Document document, int position) => Path.Combine("Files", document.BuildIdFromDocument(position));
}
