namespace ArquiveSe.Application.Models.Queries.Outputs;

public abstract class BasePagedOutput<T>
{
    public T[] Items { get; set; } = Array.Empty<T>();    

    public ushort CurrentPage { get; set; }
    public ushort TotalPages { get; set; }

    protected BasePagedOutput(ushort totalItems, ushort currentPage, ushort pageSize)
    {
        CurrentPage = currentPage;
        TotalPages = (ushort)((totalItems % pageSize == 0 ? 0 : 1) + (totalItems / pageSize));
    }
}