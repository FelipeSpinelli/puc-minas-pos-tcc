using ArquiveSe.Application.Models.Queries.Outputs;

namespace ArquiveSe.Api.Contracts.v1.Responses;
public class GetMasterListResponse : PagedResponse<GetDocumentByIdResponse>, IFromOutputConverter<GetMasterListOutput>
{
    public GetMasterListResponse()
    {
    }

    public void From(GetMasterListOutput output)
    {
        var items = new List<GetDocumentByIdResponse>(output.Items.Length);
        foreach (var item in output.Items)
        {
            var document = new GetDocumentByIdResponse();
            document.From(item);
            items.Add(document);
        }
        Items = items;
        Pagination.CurrentPage = output.CurrentPage;
        Pagination.TotalPages = output.TotalPages;
    }
}
