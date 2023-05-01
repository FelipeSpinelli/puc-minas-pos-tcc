namespace ArquiveSe.App.Services.ArquiveSeApi.Contracts.UploadFile
{
    public class UploadFileResponse
    {
        public string Id { get; set; } = null!;
        public DateTime? Expiration { get; set; }
    }
}
