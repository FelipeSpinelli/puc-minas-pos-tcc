using ArquiveSe.Domain.Abstractions.Repositories;
using Azure.Storage.Blobs;

namespace ArquiveSe.Infra.Storage
{
    internal class AzureBlobStorageRepository : IBlobRepository
    {
        private const string BLOB_CONTAINER_NAME = "managed-files";

        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Stream> GetStreamById(string id)
        {
            var content = await _blobServiceClient
                .GetBlobContainerClient(BLOB_CONTAINER_NAME)
                .GetBlobClient(id)
                .DownloadStreamingAsync();
            return content.Value.Content;
        }

        public async Task UploadFile(string id, Stream stream)
        {
            await _blobServiceClient
                .GetBlobContainerClient(BLOB_CONTAINER_NAME)
                .UploadBlobAsync(id, stream);
        }
    }
}
