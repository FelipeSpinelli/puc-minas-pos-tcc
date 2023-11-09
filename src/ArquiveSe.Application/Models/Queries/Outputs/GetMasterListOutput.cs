namespace ArquiveSe.Application.Models.Queries.Outputs;

public class GetMasterListOutput : BasePagedOutput<GetDocumentDetailOutput>
{
    public GetMasterListOutput(ushort totalItems, ushort currentPage, ushort pageSize)
        : base(totalItems, currentPage, pageSize)
    {
    }
}