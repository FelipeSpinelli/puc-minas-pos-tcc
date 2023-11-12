namespace ArquiveSe.Api.Contracts.v1.Responses;

public abstract class PagedResponse<TItem>
{
    public IEnumerable<TItem> Items { get; protected set; } = Enumerable.Empty<TItem>();
    public PaginationResponse Pagination { get; private set; } = new();
}