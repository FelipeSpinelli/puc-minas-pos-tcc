namespace ArquiveSe.App.Models.Responses;

public record PaginationResponse
{
    public ushort CurrentPage { get; set; }
    public ushort TotalPages { get; set; }
}