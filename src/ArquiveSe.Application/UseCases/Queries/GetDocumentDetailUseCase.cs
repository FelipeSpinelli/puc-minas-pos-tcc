using ArquiveSe.Application.Models.Queries.Inputs;
using ArquiveSe.Application.Models.Queries.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;

namespace ArquiveSe.Application.UseCases.Queries;

public class GetDocumentDetailUseCase : IGetDocumentDetailUseCase
{
    private readonly IDocumentReadDbPort _documentReadDbPort;

    public GetDocumentDetailUseCase(IDocumentReadDbPort documentReadDbPort)
    {
        _documentReadDbPort = documentReadDbPort;
    }

    public async Task<GetDocumentDetailOutput> Execute(GetDocumentDetailInput input)
    {
        var document = await _documentReadDbPort.GetDocumentById(input.Id);
        
        return new GetDocumentDetailOutput(document);
    }
}