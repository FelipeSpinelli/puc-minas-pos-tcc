namespace ArquiveSe.App.Models.Responses;

public abstract record PagedResponse<TItem>
{
    public IEnumerable<TItem> Items { get; set; } = Enumerable.Empty<TItem>();
    public PaginationResponse Pagination { get; set; } = new();
}
