using ArquiveSe.Api.Contracts.v1.Requests;
using ArquiveSe.Api.Contracts.v1.Responses;
using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Models.Queries.Inputs;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Application.UseCases.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ArquiveSe.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentController : ControllerBase
{
    private readonly ICommandBusPort _commandBus;
    private readonly IGetMasterListUseCase _getMasterListUseCase;
    private readonly IGetDocumentDetailUseCase _getDocumentDetailUseCase;
    private readonly IGetDocumentStreamUseCase _getDocumentStreamUseCase;

    public DocumentController(
        ICommandBusPort commandBus,
        IGetMasterListUseCase getMasterListUseCase,
        IGetDocumentDetailUseCase getDocumentDetailUseCase,
        IGetDocumentStreamUseCase getDocumentStreamUseCase)
    {
        _commandBus = commandBus;
        _getMasterListUseCase = getMasterListUseCase;
        _getDocumentDetailUseCase = getDocumentDetailUseCase;
        _getDocumentStreamUseCase = getDocumentStreamUseCase;
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

    [HttpGet("{id}", Name = "GetDocumentById")]
    public async Task<IActionResult> GetDocumentById([FromRoute] string id)
    {
        var response = new GetDocumentByIdResponse();
        var input = new GetDocumentDetailInput { Id = id };
        var detailOutput = await _getDocumentDetailUseCase.Execute(input);
        var streamOutput = await _getDocumentStreamUseCase.Execute(input);

        response.From(detailOutput);
        response.From(streamOutput);

        return Ok(response);
    }
}