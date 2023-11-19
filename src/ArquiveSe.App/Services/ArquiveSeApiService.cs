using ArquiveSe.App.Models.Requests;
using ArquiveSe.App.Services.Abstractions;
using RestEase;
using System.Net.Mime;
using System.Text;

namespace ArquiveSe.App.Services;

internal class ArquiveSeApiService : IArquiveSeApiService
{
    private record NoResponse;

    private readonly IArquiveSeApi _api;

    public ArquiveSeApiService(IArquiveSeApi api)
    {
        _api = api;
    }

    public async Task CreateDocument(CreateDocumentRequest request)
    {
        await SendAndValidate(async () =>
        {
            using var httpContent = new MultipartFormDataContent
            {
                { new StringContent(request.FolderId, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.FolderId) },
                { new StringContent(request.Name, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.Name) },
                { new StringContent(request.Type, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.Type) },
                { new StringContent(request.Approvers, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.Approvers) },
                { new StringContent(request.Reviewers, Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.Reviewers) },
                { new StringContent(request.InheritFolderPermissions.ToString().ToLower(), Encoding.UTF8, MediaTypeNames.Text.Plain), nameof(request.InheritFolderPermissions) }
            };

            using var ms = new MemoryStream();
            await request.File!.OpenReadStream().CopyToAsync(ms);
            var fileContent = new StreamContent(ms);
            httpContent.Add(fileContent, nameof(request.File), request.File!.Name);

            await _api.CreateDocument(httpContent, request.Token);

            return new NoResponse();
        });
    }

    private async Task<T> SendAndValidate<T>(Func<Task<T>> call)
    {
        try
        {
            var response = await call();
            return response;
        }
        catch (ApiException ex)
        {
            Console.WriteLine(ex.Message);
            return default;
        }
    }
}
