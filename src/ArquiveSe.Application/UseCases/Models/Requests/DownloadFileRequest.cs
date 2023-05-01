namespace ArquiveSe.Application.UseCases.Models.Requests
{
    public class DownloadFileRequest
    {
        public DownloadFileRequest(string id)
        {
            Id = id;
        }

        public string Id { get; init; }
    }
}
