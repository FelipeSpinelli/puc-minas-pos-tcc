using ArquiveSe.Api.Contracts.v1.Requests;
using ArquiveSe.Api.Contracts.v1.Responses;
using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Application.UseCases.Abstractions;
using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace ArquiveSe.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ICommandBusPort _commandBus;
    private readonly IGetMasterListUseCase _getMasterListUseCase;

    public DocumentController(
        ICommandBusPort commandBus,
        IGetMasterListUseCase getMasterListUseCase)
    {
        _commandBus = commandBus;
        _getMasterListUseCase = getMasterListUseCase;
    }

    [HttpPost(Name = "CreateDocument")]
    public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentRequest request)
    {
        request.ToInput(out CreateDocumentInput createDocumentInput);
        var output = await _commandBus.Run<CreateDocumentInput, CreateDocumentOutput>(createDocumentInput);

        request.ToInput(out IEnumerable<AddDocumentFileChunkInput> chunksInput);
        foreach (var chunkInput in chunksInput)
        {
            chunkInput.Id = output.Id;
            await _commandBus.Send(chunkInput);
        }

        return NoContent();
    }

    [HttpGet(Name = "GetMasterList")]
    public async Task<IActionResult> GetMasterList([FromQuery] GetMasterListRequest request)
    {
        var response = new GetMasterListResponse();

        request.ToInput(out var input);
        var output = await _getMasterListUseCase.Execute(input);

        response.From(output);

        return Ok(response);
    }
}