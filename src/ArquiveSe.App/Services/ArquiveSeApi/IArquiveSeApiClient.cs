using ArquiveSe.App.Services.ArquiveSeApi.Contracts.UploadFile;
using System.Text.Json;

namespace ArquiveSe.App.Services.ArquiveSeApi
{
    public interface IArquiveSeApiClient
    {
        Task<UploadFileResponse> UploadFile(UploadFileRequest request);
    }

    public class ArquiveSeApiClient : IArquiveSeApiClient
    {
        private readonly HttpClient _httpClient;

        public ArquiveSeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UploadFileResponse> UploadFile(UploadFileRequest request)
        {
            const string URL_PATH = "/api/files/upload";

            var requestContent = new MultipartFormDataContent();
            foreach (var file in request.Files)
            {
                var fileContent = new ByteArrayContent(file.Value);
                requestContent.Add(fileContent, file.Key, file.Key);
            }

            requestContent.Headers.Add("AccountId", request.AccountId);
            requestContent.Headers.Add("UserId", request.UserId);
            var response = await _httpClient.PostAsync(URL_PATH, requestContent);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"[{response.StatusCode}]{jsonResponse}");
            }

            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new ApplicationException($"[{response.StatusCode}]{jsonResponse}");
            }

            return JsonSerializer.Deserialize<UploadFileResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })!;
        }
    }
}
