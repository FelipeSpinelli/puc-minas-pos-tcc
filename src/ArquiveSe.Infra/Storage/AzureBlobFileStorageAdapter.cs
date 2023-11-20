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
        var blobClient = _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .GetBlobClient(document.BuildIdFromDocument());

        if (!await blobClient.ExistsAsync())
        {
            return Array.Empty<byte>();
        }

        var content = await blobClient.DownloadStreamingAsync();

        using var ms = new MemoryStream();
        content.Value.Content.CopyTo(ms);
        return ms.ToArray();
    }

    public async Task Save(Document document, byte[] stream)
    {
        var id = document.BuildIdFromDocument();
        using var ms = new MemoryStream();
        ms.Write(stream, 0, stream.Length);
        ms.Position = 0;

        var blobClient = _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .GetBlobClient(id);

        await blobClient.DeleteIfExistsAsync();

        await _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .UploadBlobAsync(id, ms);
    }
}
