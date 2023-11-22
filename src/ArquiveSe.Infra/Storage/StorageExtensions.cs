using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Infra.Storage;

public static class StorageExtensions
{
    public static string BuildIdFromDocument(this Document document) => $"{document.Id}.{document.Type}";
    public static string BuildIdFromDocument(this Document document, int position) => $"{document.Id}-{position}.{document.Type}";
}
