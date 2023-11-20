using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Infra.Storage;

public class LocalFileStorageAdapter : IFileStoragePort
{
    public async Task<byte[]> Load(Document document)
    {
        using var ms = new MemoryStream();
        using var fs = File.OpenRead(GetDocumentFilePath(document));
        await fs.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task Save(Document document, byte[] stream)
    {
        using var fs = File.OpenWrite(GetDocumentFilePath(document))!;
        await fs.WriteAsync(stream);
    }

    private static string GetDocumentFilePath(Document document) => Path.Combine("Files", document.BuildIdFromDocument());
}
