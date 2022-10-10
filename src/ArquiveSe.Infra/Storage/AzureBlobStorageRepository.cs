using ArquiveSe.Domain.Abstractions.Repositories;
using Azure.Storage.Blobs;

namespace ArquiveSe.Infra.Storage
{
    internal class AzureBlobStorageRepository : IBlobRepository
    {
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobStorageRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Stream> GetStreamById(string id)
        {
            var content = await _blobServiceClient
                .GetBlobContainerClient("managed-files")
                .GetBlobClient(id)
                .DownloadStreamingAsync();
            return content.Value.Content;
        }

        public async Task UploadFile(string id, Stream stream)
        {
            stream.Position = 0;
            await _blobServiceClient
                .GetBlobContainerClient("managed-files")
                .UploadBlobAsync(id, stream);
        }
    }
}
