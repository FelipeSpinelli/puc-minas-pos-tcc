namespace ArquiveSe.Domain.Abstractions.Repositories
{
    public interface IBlobRepository 
    {
        Task<Stream> GetStreamById(string id);
        Task UploadFile(string id, Stream stream);
    }
}
