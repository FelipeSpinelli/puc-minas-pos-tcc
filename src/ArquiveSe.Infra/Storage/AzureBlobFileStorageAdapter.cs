using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ArquiveSe.Infra.Storage;
public class AzureBlobFileStorageAdapter : IFileStoragePort
{
    private const string BLOB_CONTAINER_NAME = "managed-files";

    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobFileStorageAdapter(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task JoinChunks(Document document)
    {
        using var ms = new MemoryStream();
        for (ulong i = 0; i < document.File.ExpectedChunks; i++)
        {
            var content = await GetBlobResponse(document.BuildIdFromDocument((int)i));
            content!.Value.Content.CopyTo(ms);
        }
        ms.Position = 0;

        var id = document.BuildIdFromDocument();
        var blobClient = _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .GetBlobClient(id);

        await blobClient.DeleteIfExistsAsync();

        await _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .UploadBlobAsync(id, ms);
    }

    public async Task SaveChunk(Document document, int position, byte[] stream)
    {
        var id = document.BuildIdFromDocument(position);
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

    public async Task<byte[]> Load(Document document)
    {
        var blobId = document.BuildIdFromDocument();

        var content = await GetBlobResponse(blobId);
        if (content is null)
        {
            return Array.Empty<byte>();
        }
        
        using var ms = new MemoryStream();
        content.Value.Content.CopyTo(ms);
        return ms.ToArray();
    }

    private async Task<Response<BlobDownloadStreamingResult>?> GetBlobResponse(string blobId)
    {
        var blobClient = _blobServiceClient
            .GetBlobContainerClient(BLOB_CONTAINER_NAME)
            .GetBlobClient(blobId);

        if (!await blobClient.ExistsAsync())
        {
            return null;
        }

        return await blobClient.DownloadStreamingAsync();
    }
}
