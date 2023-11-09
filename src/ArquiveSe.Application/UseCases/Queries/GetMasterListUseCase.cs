using ArquiveSe.Application.Models.Queries.Inputs;
using ArquiveSe.Application.Models.Queries.Outputs;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Application.UseCases.Abstractions;

namespace ArquiveSe.Application.UseCases.Queries;

public class GetMasterListUseCase : IGetMasterListUseCase
{
    private readonly IDocumentReadDbPort _documentReadDbPort;

    public GetMasterListUseCase(IDocumentReadDbPort documentReadDbPort)
    {
        _documentReadDbPort = documentReadDbPort;
    }

    public async Task<GetMasterListOutput> Execute(GetMasterListInput input)
    {
        var (documents, count) = await _documentReadDbPort.GetMasterList(input.Query, input.Offset, input.Size);
        return new GetMasterListOutput(count, input.Page, input.Size)
        {
            Items = documents.Select(x => new GetDocumentDetailOutput(x)).ToArray()
        };
    }
}