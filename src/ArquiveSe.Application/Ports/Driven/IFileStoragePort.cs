using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Application.Ports.Driven;

public interface IFileStoragePort
{
    Task Save(Document document, byte[] stream);
    Task<byte[]> Load(Document document);
}