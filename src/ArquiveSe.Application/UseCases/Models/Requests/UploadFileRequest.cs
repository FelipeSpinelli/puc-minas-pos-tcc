using Microsoft.AspNetCore.Http;

namespace ArquiveSe.Application.UseCases.Models.Requests
{
    public class UploadFileRequest
    {
        public string AccountId { get; init; }
        public string UserId { get; init; }
        public IFormFile File { get; init; }

        public UploadFileRequest(
            string accountId, 
            string userId, 
            IFormCollection form)
        {
            AccountId = accountId;
            UserId = userId;
            File = form.Files.First();
        }
    }
}
