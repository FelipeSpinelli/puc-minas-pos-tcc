using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;
using Azure.Storage.Blobs;

namespace ArquiveSe.Infra.Storage;

public class AzureBlobFileStorageAdapter : IFileStoragePort
{
    private const string BLOB_CONTAINER_NAME = "managed-files";

    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobFileStorageAdapter(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<byte[]> Load(Document document)
    {
        var content = await _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .GetBlobClient(BuildIdFromDocument(document))
            .DownloadStreamingAsync();

        using var ms = new MemoryStream();
        content.Value.Content.CopyTo(ms);
        return ms.ToArray();
    }

    public async Task Save(Document document, byte[] stream)
    {
        using var ms = new MemoryStream();
        ms.Write(stream, 0, stream.Length);
        await _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .UploadBlobAsync(BuildIdFromDocument(document), ms);
    }

    private static string BuildIdFromDocument(Document document) => $"{document.Id}.{document.Type}";
}
