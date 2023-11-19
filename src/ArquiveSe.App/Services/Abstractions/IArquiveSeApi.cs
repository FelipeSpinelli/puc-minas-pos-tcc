using RestEase;

namespace ArquiveSe.App.Services.Abstractions;

public interface IArquiveSeApi
{
    [Post("Document")]
    Task CreateDocument([Body] HttpContent content, [Header("Authorization")] string token);
}
