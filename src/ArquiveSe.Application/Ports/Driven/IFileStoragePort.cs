using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Application.Ports.Driven;

public interface IFileStoragePort
{
    Task SaveChunk(Document document, int position, byte[] stream);
    Task JoinChunks(Document document);
    Task<byte[]> Load(Document document);
}