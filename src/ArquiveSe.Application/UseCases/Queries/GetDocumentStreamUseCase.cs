using ArquiveSe.Application.Models.Queries.Inputs;
using ArquiveSe.Application.Models.Queries.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;
using ArquiveSe.Domain.Entities;

namespace ArquiveSe.Application.UseCases.Queries;

public class GetDocumentStreamUseCase : IGetDocumentStreamUseCase
{
    private readonly IPersistenceDbPort _persistenceDbPort;
    private readonly IFileStoragePort _fileStoragePort;

    public GetDocumentStreamUseCase(IPersistenceDbPort persistenceDbPort, IFileStoragePort fileStoragePort)
    {
        _persistenceDbPort = persistenceDbPort;
        _fileStoragePort = fileStoragePort;
    }

    public async Task<GetDocumentStreamOutput> Execute(GetDocumentDetailInput input)
    {
        var document = await _persistenceDbPort.LoadAggregate<Document>(input.Id) ?? 
            throw new ApplicationException($"Document {input.Id} was not found!");
        var fileBytes = await _fileStoragePort.Load(document);

        return new GetDocumentStreamOutput
        {
            Id = document.Id,
            Name = $"{document.Name}.{document.Type}",
            Stream = fileBytes
        };
    }
}
