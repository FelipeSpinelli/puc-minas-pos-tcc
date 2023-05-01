namespace ArquiveSe.App.Services.ArquiveSeApi.Contracts.UploadFile
{
    public class UploadFileRequest
    {
        public string? UserId { get; set; }
        public string? AccountId { get; set; }
        public IDictionary<string, byte[]> Files { get; set; } = new Dictionary<string, byte[]>(0);
    }
}
